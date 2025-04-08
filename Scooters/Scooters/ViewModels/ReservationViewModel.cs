using System.Collections.ObjectModel;
using Application.Common.Interfaces.CurrentUserProvider;
using Application.Reservations.Commands.CreateReservation;
using Application.Rides.Commands.CreateRide;
using Application.Scooters.Queries.GetScooterById;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using MediatR;
using Scooters.Views;

namespace Scooters.ViewModels;

[QueryProperty(nameof(ScooterId), "scooterId")]
public partial class ReservationViewModel : ObservableObject
{
    [ObservableProperty] private Reservation _reservation;
    [ObservableProperty] private ObservableCollection<int> _timeSlots;
    [ObservableProperty] private Guid _scooterId;
    [ObservableProperty] private Scooter _scooter;
    
    private readonly IMediator _mediator;
    private readonly ICurrentUserProvider _currentUserProvider;
    
    public ReservationViewModel(IMediator mediator, ICurrentUserProvider currentUserProvider)
    {
        TimeSlots = new ObservableCollection<int>();
        Reservation = new Reservation();
        _mediator = mediator;
        _currentUserProvider = currentUserProvider;
    }

    [RelayCommand]
    private async Task Reserve()
    {
        Reservation.StartTime = DateTime.Now;
        
        var response = await _mediator.Send(new CreateReservationCommand(Reservation));
        if (response.IsSuccessful)
        {
            await Shell.Current.DisplayAlert("Success", "Reservation created successfully", "OK");
            await Shell.Current.Navigation.PopAsync();
            await Shell.Current.GoToAsync("//MainPage");
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
        var user = _currentUserProvider.GetCurrentUser();
        var ride = new Ride
        {
            StartTime = DateTime.Now,
            ScooterId = ScooterId,
            UserId = user!.Value
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
        await Shell.Current.GoToAsync(nameof(RideView), parameters);
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
        Reservation.UserId = _currentUserProvider.GetCurrentUser()!.Value;
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
            await Shell.Current.Navigation.PopAsync();
        }
    }
}
