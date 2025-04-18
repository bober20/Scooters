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

namespace Scooters.ViewModels;

public partial class ReservationViewModel : ObservableObject
{
    [ObservableProperty] private Reservation _reservation = new();
    [ObservableProperty] private ObservableCollection<int> _timeSlots = new();
    [ObservableProperty] private Guid _scooterId;
    [ObservableProperty] private Scooter _scooter;

    private Guid _currentUserId;
    
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
        if (Reservation.Duration == 0)
        {
            return;
        }
        
        Reservation.StartTime = DateTime.Now;

        var response = await _mediator.Send(new CreateReservationCommand(Reservation));
        if (response.IsSuccessful)
        {
            await ShowNotification();
            await _navigationService.NavigateToMainPageAsync();
            await _navigationService.ClosePopupAsync();
            return;
        }

        await _navigationService.ShowAlertAsync("Error", response.ErrorMessage);
    }

    [RelayCommand]
    private async Task Appearing()
    {
        await GetCurrentUserAsync();
        await InitializeScooter();
        InitializeTimeSlots();
        InitializeReservation();
    }

    [RelayCommand]
    private async Task RideLink()
    {
        var ride = new Ride
        {
            StartTime = DateTime.Now,
            ScooterId = ScooterId,
            UserId = _currentUserId
        };
        var response = await _mediator.Send(new CreateRideCommand(ride));
        if (!response.IsSuccessful)
        {
            await _navigationService.ShowAlertAsync("Error", response.ErrorMessage);
            return;
        }

        await _navigationService.ClosePopupAsync();
        await _navigationService.ShowPopupAsync<RideViewModel>(
            onPresenting: viewModel => viewModel.RideId = response.Data);
    }
    
    private async Task GetCurrentUserAsync()
    {
        if (_currentUserProvider.GetCurrentUser() is not Guid userId)
        {
            await _navigationService.NavigateToLoginPageAsync();
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
        Reservation.ScooterId = ScooterId;
        Reservation.UserId = _currentUserId;
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
            await _navigationService.ShowAlertAsync("Error", "There is no scooter with this ID");
            await _navigationService.ClosePopupAsync();
        }
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