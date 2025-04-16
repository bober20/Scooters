using Application.Common.Interfaces.NavigationService;
using Application.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;

namespace Scooters.ViewModels;

public partial class RideViewModel : ObservableObject
{
    [ObservableProperty] private Ride _ride;
    [ObservableProperty] private Guid _rideId;
    [ObservableProperty] private string _countdown;
    [ObservableProperty] private string _distance = "0.00";
    
    private System.Timers.Timer _timer;

    private readonly RideService _rideService;
    private readonly INavigationService _navigationService;

    public RideViewModel(INavigationService navigationService, RideService rideService)
    {
        _navigationService = navigationService;
        _rideService = rideService;
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
        await _rideService.EndRideAsync(Ride.Id);
        await _navigationService.ClosePopupAsync();
    }

    private async Task InitializeRideAsync()
    {
        var response = await _rideService.GetRideByIdAsync(RideId);
        if (response.IsSuccessful)
        {
            Ride = response.Data;
            return;
        }

        await _navigationService.ClosePopupAsync();
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
}