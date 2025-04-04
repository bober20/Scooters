using System.Collections.ObjectModel;
using Application.Common.Interfaces.CurrentUserProvider;
using Application.Reservations.Commands.EndReservation;
using Application.Reservations.Queries.GetReservationByFilter;
using Application.Scooters.Commands.CreateScooter;
using Application.Scooters.Queries.GetAllScooters;
using Application.Scooters.Queries.GetScooterById;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using MediatR;

namespace Scooters.ViewModels;

public partial class MainViewModel : ObservableObject 
{
    [ObservableProperty] private Scooter _scooter = new Scooter();
    [ObservableProperty] private Guid _currentUser;
    [ObservableProperty] private Reservation _reservation;
    [ObservableProperty] private bool _hasReservation;
    [ObservableProperty] private bool _countdown;
    public ObservableCollection<Scooter> Scooters { get; set; }
    private IMediator _mediator;
    private readonly ICurrentUserProvider _currentUserProvider;

    public MainViewModel(IMediator mediator, ICurrentUserProvider currentUserProvider)
    {
        _mediator = mediator;
        _currentUserProvider = currentUserProvider;
    }

    [RelayCommand]
    private async Task Appearing()
    {
        GetCurrentUser();
        await FetchScooters();
        await FetchReservation();
    }

    [RelayCommand]
    private async Task ReservationPressed()
    {
        var result = await Shell.Current.DisplayActionSheet("Reservation", "Cancel", null,
            "Cancel Reservation");
        if (result is "Cancel Reservation")
        {
            await _mediator.Send(new EndReservationCommand(Reservation.Id));
        }
    }

    [RelayCommand]
    private async Task AddScooter(Scooter scooter)
    {
        scooter.Coordinates = new Coordinates() {Latitude = 0, Longitude = 0};
        await _mediator.Send(new CreateScooterCommand(scooter));
    }
    
    [RelayCommand]
    private async Task MapPageLink()
    {
        await Shell.Current.GoToAsync("//MapPage");
    }
    
    private async Task FetchScooters()
    {
        var response = await _mediator.Send(new GetAllScootersQuery());
        if (response.IsSuccessful)
        {
            Scooters = new ObservableCollection<Scooter>(response.Data);
        }
        else
        {
            Scooters = new ObservableCollection<Scooter>();
        }
    }

    private async Task FetchReservation()
    {
        var reservation = await _mediator.Send(new GetReservationQuery(
            r => r.UserId == CurrentUser && r.IsActive));
        if (!reservation.IsSuccessful)
        {
            HasReservation = false;
        }
        else
        {
            HasReservation = true;
            Reservation = reservation.Data[0];
        }
    }
    
    private void GetCurrentUser()
    {
        CurrentUser = _currentUserProvider.GetCurrentUser().Value;
    }
}