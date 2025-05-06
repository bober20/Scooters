using System.Collections.ObjectModel;
using Application.Common.Interfaces;
using Application.Reservations.Commands.CreateReservation;
using Application.Rides.Commands.CreateRide;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using MediatR;
using Plugin.LocalNotification;
using Scooters.Common.Interfaces;

namespace Scooters.ViewModels;

public partial class ReservationViewModel(
    IMediator mediator,
    ICurrentUserProvider currentUserProvider,
    INavigationService navigationService,
    INotificationService notificationService)
    : ObservableObject
{
    [ObservableProperty] private Reservation _reservation = new();
    [ObservableProperty] private ObservableCollection<int> _timeSlots = new();
    [ObservableProperty] private Scooter? _scooter;
    [ObservableProperty] private string _distance = string.Empty;

    private Guid _currentUserId;
    private Location? _userLocation;

    private IMinutesSheetService? _minutesSheetService;
    private IBottomSheetService? _bottomSheetService;

    [RelayCommand]
    private void Loaded()
    {
        _bottomSheetService?.ShowBottomSheetCall();
    }

    [RelayCommand]
    private void ShowMinutesBottomSheet()
    {
        _minutesSheetService?.ShowMinutesBottomSheetCall();
    }

    [RelayCommand]
    private async Task ReserveScooter()
    {
        if (Reservation.Duration == 0)
        {
            await GoToRidePage();
            return;
        }

        Reservation.StartTime = DateTime.Now;

        var response = await mediator.Send(new CreateReservationCommand(Reservation));
        if (response.IsSuccessful)
        {
            await ShowReservationNotificationAsync();
            _bottomSheetService?.CloseBottomSheetCall();
            await navigationService.GoBackAsync();
            return;
        }

        await Shell.Current.DisplayAlert("Error", response.ErrorMessage, "OK");
    }

    [RelayCommand]
    private async Task Appearing()
    {
        await GetCurrentUserAsync();
        InitializeTimeSlots();
        InitializeReservation();
        _userLocation = await GetUserLocationAsync();
        GetScooterDistance();
    }

    private async Task GoToRidePage()
    {
        if (Scooter is null) return;
        var ride = new Ride
        {
            StartTime = DateTime.Now,
            ScooterId = Scooter.Id,
            UserId = _currentUserId
        };
        var response = await mediator.Send(new CreateRideCommand(ride));
        if (!response.IsSuccessful)
        {
            await Shell.Current.DisplayAlert("Error", response.ErrorMessage, "OK");
            return;
        }

        _bottomSheetService?.CloseBottomSheetCall();
        if (response.Data is null)
        {
            await Shell.Current.DisplayAlert("Error", "Ride creation failed", "OK");
            return;
        }

        await navigationService.GoBackAndNavigateToRidePageAsync(response.Data);
    }

    public void SetBottomSheetService(IBottomSheetService bottomSheetService)
    {
        _bottomSheetService = bottomSheetService;
    }

    public void SetMinutesBottomSheetService(IMinutesSheetService minutesSheetService)
    {
        _minutesSheetService = minutesSheetService;
    }

    public void SetScooter(Scooter scooter)
    {
        Scooter = scooter;
    }

    private void GetScooterDistance()
    {
        if (_userLocation is null || Scooter is null)
        {
            Distance = "Enable location service";
            return;
        }

        var scooterLocation = new Location(Scooter.Coordinates.Latitude, Scooter.Coordinates.Longitude);
        var distance = Location.CalculateDistance(_userLocation, scooterLocation, DistanceUnits.Kilometers);
        Distance = $"{Math.Round(distance, 3)} km";
    }

    private async Task<Location?> GetUserLocationAsync()
    {
        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                    return null;
            }

            var location = await Geolocation.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Medium,
                Timeout = TimeSpan.FromSeconds(10)
            });

            if (location != null)
            {
                _userLocation = new Location(location.Latitude, location.Longitude);
                return _userLocation;
            }
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("Error",
                "Could not fetch user's location. Distance to the scooter won't be displayed", "OK");
        }

        return null;
    }

    private async Task GetCurrentUserAsync()
    {
        if (currentUserProvider.GetCurrentUser() is not { } userId)
        {
            _bottomSheetService?.CloseBottomSheetCall();
            await navigationService.GoBackAsync();
            return;
        }

        _currentUserId = userId;
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
        if (Scooter is null) return;
        Reservation.ScooterId = Scooter.Id;
        Reservation.UserId = _currentUserId;
    }

    private async Task ShowReservationNotificationAsync()
    {
        if (await notificationService.AreNotificationsEnabled() == false)
        {
            await notificationService.RequestNotificationPermission();
        }

        var request = new NotificationRequest
        {
            NotificationId = 3333,
            Title = "Reservation",
            Description = $"You have created a reservation for {Reservation.Duration} mins"
        };

        await notificationService.Show(request);
    }
}