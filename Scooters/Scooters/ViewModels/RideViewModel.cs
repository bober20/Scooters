using Application.Common.Interfaces;
using Application.Rides.Commands.EndRide;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using MediatR;
using Scooters.Common.Interfaces;

namespace Scooters.ViewModels;

public partial class RideViewModel(IMediator mediator, INavigationService navigationService) : ObservableObject
{
    [ObservableProperty] private Ride? _ride;
    [ObservableProperty] private string _countdown = string.Empty;
    [ObservableProperty] private string _distance = "0.00";

    private System.Timers.Timer? _timer;

    private IBottomSheetService? _bottomSheetService;

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
        _bottomSheetService?.ShowBottomSheetCall();
    }

    [RelayCommand]
    private void Appearing()
    {
        InitialiseTimer();
    }

    [RelayCommand]
    private async Task EndRide()
    {
        if (Ride is null) return;
        await mediator.Send(new EndRideCommand(Ride.Id));

        _bottomSheetService?.CloseBottomSheetCall();
        await navigationService.GoBackAsync();
    }

    private void InitialiseTimer()
    {
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = true;
        _timer.Start();
    }

    private void OnTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        if (Ride is null)
        {
            Shell.Current.DisplayAlert("Ride error", "Ride is not specified", "OK");
            return;
        }

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