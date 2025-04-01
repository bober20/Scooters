using System.Collections.ObjectModel;
using Application.Reservations.Commands.CreateReservation;
using Application.Scooters.Queries.GetScooterById;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using MediatR;

namespace Scooters.ViewModels;

[QueryProperty(nameof(ScooterId), "scooterId")]
public partial class ReservationViewModel : ObservableObject
{
    [ObservableProperty] private Reservation _reservation;
    [ObservableProperty] private ObservableCollection<int> _timeSlots;
    [ObservableProperty] private Guid _scooterId;
    private readonly IMediator _mediator;
    
    
    public ReservationViewModel(IMediator mediator)
    {
        TimeSlots = new ObservableCollection<int>();
        Reservation = new Reservation();
        _mediator = mediator;
    }

    [RelayCommand]
    private async Task Reserve()
    {
        Reservation.StartTime = DateTime.Now;
        await _mediator.Send(new CreateReservationCommand(Reservation));
    }
    
    [RelayCommand]
    private void Appearing()
    {
        InitializeTimeSlots();
        InitializeReservation();
    }
    
    private void InitializeTimeSlots()
    {
        TimeSlots.Clear();
        for (int i = 0; i < 60; i += 10)
        {
            TimeSlots.Add(i);
        }
    }
    
    private void InitializeReservation()
    {
        Reservation.ScooterId = ScooterId;
    }
}
