using System.Collections.ObjectModel;
using Application.Common.Interfaces.CurrentUserProvider;
using Application.Common.Interfaces.NavigationService;
using Application.Reservations.Commands.CreateReservation;
using Application.Rides.Commands.CreateRide;
using Application.Scooters.Queries.GetScooterById;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using MediatR;
using Plugin.LocalNotification;
using Scooters.Views;

namespace Scooters.ViewModels;

[QueryProperty(nameof(ScooterId), "scooterId")]
public partial class ReservationViewModel : ObservableObject
{
    [ObservableProperty] private Reservation _reservation = new();
    [ObservableProperty] private ObservableCollection<int> _timeSlots = new();
    [ObservableProperty] private Guid _scooterId;
    [ObservableProperty] private Scooter _scooter;
    
    private readonly IMediator _mediator;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly INavigationService _navigationService;
    private readonly INotificationService _notificationService;
    
    public ReservationViewModel(IMediator mediator, 
        ICurrentUserProvider currentUserProvider, INavigationService navigationService, 
        INotificationService notificationService)
    {
        _mediator = mediator;
        _currentUserProvider = currentUserProvider;
        _navigationService = navigationService;
        _notificationService = notificationService;
    }

    [RelayCommand]
    private async Task Reserve()
    {
        Reservation.StartTime = DateTime.Now;
        
        var response = await _mediator.Send(new CreateReservationCommand(Reservation));
        if (response.IsSuccessful)
        {
            await ShowNotification();
            await _navigationService.NavigateToAsync("//MainPage");
            return;
        }
        
        await Shell.Current.DisplayAlert("Error", response.ErrorMessage, "OK");
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
        var response = await _mediator.Send(new CreateRideCommand(ride));
        if (!response.IsSuccessful)
        {
            await Shell.Current.DisplayAlert("Error", response.ErrorMessage, "OK");
            return;
        }
        Dictionary<string, object> parameters = new()
        {
            { "rideId", response.Data }
        };
        
        await _navigationService.NavigateToAsync("RidePage", parameters);
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
        var scooter = await _mediator.Send(new GetScooterQuery(ScooterId));
        if (scooter.IsSuccessful)
        {
            Scooter = scooter.Data;
            Reservation.Scooter = scooter.Data;
        }
        else
        {
            await _navigationService.GoBackAsync();
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
