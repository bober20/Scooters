using Application.Common.Interfaces;
using Application.Rides.Commands.EndRide;
using Application.Rides.Queries.GetRideById;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using MediatR;
using Scooters.Common.Interfaces;

namespace Scooters.ViewModels;

public partial class RideViewModel : ObservableObject
{
    [ObservableProperty] private Ride _ride;
    [ObservableProperty] private string _countdown;
    [ObservableProperty] private string _distance = "0.00";

    private System.Timers.Timer _timer;

    private IBottomSheetService _bottomSheetService;
    private readonly IMediator _mediator;
    private readonly INavigationService _navigationService;

    public RideViewModel(IMediator mediator, INavigationService navigationService)
    {
        _mediator = mediator;
        _navigationService = navigationService;
    }

    public void SetBottomSheetService(IBottomSheetService bottomSheetService)
    {
        _bottomSheetService = bottomSheetService;
    }

    public void SetRide(Ride ride)
    {
        Ride = ride;
    }

    [RelayCommand]
    private void Loaded()
    {
        _bottomSheetService.ShowBottomSheetCall();
    }

    [RelayCommand]
    private async Task Appearing()
    {
        InitialiseTimer();
    }

    [RelayCommand]
    private async Task EndRide()
    {
        await _mediator.Send(new EndRideCommand(Ride.Id));

        _bottomSheetService.CloseBottomSheetCall();
        await _navigationService.GoBackAsync();
    }

    private void InitialiseTimer()
    {
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = true;
        _timer.Start();
    }

    private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        MainThread.InvokeOnMainThreadAsync(() =>
        {
            var timePassed = DateTime.Now - Ride.StartTime;
            Countdown = $"{timePassed.Minutes:D2}:{timePassed.Seconds:D2}";

            double.TryParse(Distance, out var currentDistance);
            var distance = timePassed.TotalSeconds * 4;
            if (distance - currentDistance > 15)
            {
                Distance = $"{distance:F2}";
            }
        });
    }
}