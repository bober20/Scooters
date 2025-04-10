using Application.Common.Interfaces.NavigationService;
using Application.Rides.Commands.EndRide;
using Application.Rides.Queries.GetRideById;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using MediatR;

namespace Scooters.ViewModels;

[QueryProperty(nameof(RideId), "rideId")]
public partial class RideViewModel : ObservableObject
{
    [ObservableProperty] private Ride _ride;
    [ObservableProperty] private Guid _rideId;
    [ObservableProperty] private string _countdown;
    [ObservableProperty] private string _distance = "0.00";
    private System.Timers.Timer _timer;

    private readonly IMediator _mediator;
    private readonly INavigationService _navigationService;
    
    public RideViewModel(IMediator mediator, INavigationService navigationService)
    {
        _mediator = mediator;
        _navigationService = navigationService;
    }
    
    [RelayCommand]
    private async Task Appearing()
    {
        await InitializeRideAsync();
        InitialiseTimer();
    }

    [RelayCommand]
    private async Task EndRide()
    {
        await _mediator.Send(new EndRideCommand(Ride.Id));
        await _navigationService.GoBackAsync();
    }

    private async Task InitializeRideAsync()
    {
        var response = await _mediator.Send(new GetRideQuery(RideId));
        if (response.IsSuccessful)
        {
            Ride = response.Data;
            return;
        }

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

            var distance = timePassed.TotalSeconds * 4.1;
            Distance = $"{distance:F2}";
        });
    }

    public void StopTimer()
    {
        _timer.Stop();
        _timer.Dispose();
    }
}