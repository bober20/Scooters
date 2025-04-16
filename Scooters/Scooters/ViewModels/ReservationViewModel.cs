using System.Collections.ObjectModel;
using Application.Common.Interfaces.CurrentUserProvider;
using Application.Common.Interfaces.NavigationService;
using Application.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using Plugin.LocalNotification;

namespace Scooters.ViewModels;

public partial class ReservationViewModel : ObservableObject
{
    [ObservableProperty] private Reservation _reservation = new();
    [ObservableProperty] private ObservableCollection<int> _timeSlots = new();
    [ObservableProperty] private Guid _scooterId;
    [ObservableProperty] private Scooter _scooter;

    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly INavigationService _navigationService;
    private readonly INotificationService _notificationService;
    private readonly ReservationService _reservationService;
    private readonly RideService _rideService;
    private readonly ScooterService _scooterService;

    public ReservationViewModel(
        ICurrentUserProvider currentUserProvider, INavigationService navigationService,
        INotificationService notificationService, ReservationService reservationService, RideService rideService, ScooterService scooterService)
    {
        _currentUserProvider = currentUserProvider;
        _navigationService = navigationService;
        _notificationService = notificationService;
        _reservationService = reservationService;
        _rideService = rideService;
        _scooterService = scooterService;
    }

    [RelayCommand]
    private async Task Reserve()
    {
        if (Reservation.Duration == 0)
        {
            return;
        }
        
        Reservation.StartTime = DateTime.Now;

        var response = await _reservationService.CreateReservationAsync(Reservation);
        if (response.IsSuccessful)
        {
            await ShowNotification();
            await _navigationService.NavigateToAsync("//MainPage");
            await _navigationService.ClosePopupAsync();
            return;
        }

        await _navigationService.ShowAlertAsync("Error", response.ErrorMessage);
    }

    [RelayCommand]
    private async Task Appearing()
    {
        await InitializeScooter();
        InitializeTimeSlots();
        InitializeReservation();
    }

    [RelayCommand]
    private async Task RideLink()
    {
        if (_currentUserProvider.GetCurrentUser() is not Guid userId)
        {
            return;
        }

        var ride = new Ride
        {
            StartTime = DateTime.Now,
            ScooterId = ScooterId,
            UserId = userId
        };
        var response = await _rideService.CreateRideAsync(ride);
        if (!response.IsSuccessful)
        {
            await _navigationService.ShowAlertAsync("Error", response.ErrorMessage);
            return;
        }

        await _navigationService.ClosePopupAsync();
        await _navigationService.ShowPopupAsync<RideViewModel>(
            onPresenting: viewModel => viewModel.RideId = response.Data);
    }

    private void InitializeTimeSlots()
    {
        TimeSlots.Clear();
        for (int i = 10; i < 60; i += 10)
        {
            TimeSlots.Add(i);
        }
    }

    private void InitializeReservation()
    {
        Reservation.ScooterId = ScooterId;
        if (_currentUserProvider.GetCurrentUser() is not Guid userId)
        {
            return;
        }

        Reservation.UserId = userId;
    }

    private async Task InitializeScooter()
    {
        var scooter = await _scooterService.GetScooterByIdAsync(ScooterId);
        if (scooter.IsSuccessful)
        {
            Scooter = scooter.Data;
            Reservation.Scooter = scooter.Data;
        }
        else
        {
            await _navigationService.ClosePopupAsync();
        }
    }

    private async Task ShowNotification()
    {
        if (await _notificationService.AreNotificationsEnabled() == false)
        {
            await _notificationService.RequestNotificationPermission();
        }

        var request = new NotificationRequest()
        {
            NotificationId = 3333,
            Title = "Reservation",
            Description = $"You have created a reservation for {Reservation.Duration} mins",
        };

        await _notificationService.Show(request);
    }
}