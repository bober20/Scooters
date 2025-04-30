using System.Collections.ObjectModel;
using Application.Common.Interfaces;
using Application.Reservations.Commands.CreateReservation;
using Application.Rides.Commands.CreateRide;
using Application.Scooters.Queries.GetScooterById;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using MediatR;
using Plugin.LocalNotification;
using Scooters.Common.Interfaces;

namespace Scooters.ViewModels;

public partial class ReservationViewModel : ObservableObject
{
    [ObservableProperty] private Reservation _reservation = new();
    [ObservableProperty] private ObservableCollection<int> _timeSlots = new();
    [ObservableProperty] private Scooter _scooter;
    [ObservableProperty] private string _distance;

    private Guid _currentUserId;
    private Location _userLocation;

    private IMinutesSheetService _minutesSheetService;
    private IBottomSheetService _bottomSheetService;
    private readonly IMediator _mediator;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly INavigationService _navigationService;
    private readonly INotificationService _notificationService;

    public ReservationViewModel(IMediator mediator, ICurrentUserProvider currentUserProvider,
        INavigationService navigationService, INotificationService notificationService)
    {
        _mediator = mediator;
        _currentUserProvider = currentUserProvider;
        _navigationService = navigationService;
        _notificationService = notificationService;
    }

    [RelayCommand]
    private void Loaded()
    {
        _bottomSheetService.ShowBottomSheetCall();
    }
    
    [RelayCommand]
    private void ShowMinutesBottomSheet()
    {
        _minutesSheetService.ShowMinutesBottomSheetCall();
    }
        
    [RelayCommand]
    private async Task ReserveScooter()
    {
        if (Reservation.Duration == 0)
        {
            return;
        }

        Reservation.StartTime = DateTime.Now;

        var response = await _mediator.Send(new CreateReservationCommand(Reservation));
        if (response.IsSuccessful)
        {
            await ShowNotification();
            _bottomSheetService.CloseBottomSheetCall();
            await _navigationService.GoBackAsync();
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
        GetDistance();
    }

    [RelayCommand]
    private async Task RideLink()
    {
        var ride = new Ride
        {
            StartTime = DateTime.Now,
            ScooterId = Scooter.Id,
            UserId = _currentUserId
        };
        var response = await _mediator.Send(new CreateRideCommand(ride));
        if (!response.IsSuccessful)
        {
            await Shell.Current.DisplayAlert("Error", response.ErrorMessage, "OK");
            return;
        }

        _bottomSheetService.CloseBottomSheetCall();
        await _navigationService.GoBackAndNavigateToRidePageAsync(response.Data);
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

    private void GetDistance()
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

    private async Task<Location> GetUserLocationAsync()
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
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error",
                "Could not fetch user's location. Distance to the scooter won't be displayed", "OK");
        }

        return null;
    }

    private async Task GetCurrentUserAsync()
    {
        if (_currentUserProvider.GetCurrentUser() is not Guid userId)
        {
            _bottomSheetService.CloseBottomSheetCall();
            await _navigationService.GoBackAsync();
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
        Reservation.ScooterId = Scooter.Id;
        Reservation.UserId = _currentUserId;
    }

    private async Task ShowNotification()
    {
        if (await _notificationService.AreNotificationsEnabled() == false)
        {
            await _notificationService.RequestNotificationPermission();
        }

        var request = new NotificationRequest
        {
            NotificationId = 3333,
            Title = "Reservation",
            Description = $"You have created a reservation for {Reservation.Duration} mins"
        };

        await _notificationService.Show(request);
    }
}