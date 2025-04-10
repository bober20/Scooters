using System.Collections.ObjectModel;
using Application.Common.Interfaces.CurrentUserProvider;
using Application.Common.Interfaces.NavigationService;
using Application.Reservations.Commands.EndReservation;
using Application.Reservations.Queries.GetReservationByUser;
using Application.Rides.Commands.CreateRide;
using Application.Rides.Queries.GetRidesByFilter;
using Application.Scooters.Commands.CreateScooter;
using Application.Scooters.Queries.GetAllScooters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using MediatR;
using Scooters.Views;

namespace Scooters.ViewModels;

public partial class MainViewModel : ObservableObject 
{
    [ObservableProperty] private Guid _currentUserId;
    [ObservableProperty] private Reservation? _reservation;
    [ObservableProperty] private List<Ride> _rides;
    [ObservableProperty] private bool _hasReservation;
    [ObservableProperty] private bool _hasRides;
    [ObservableProperty] private string _countdown;
    public ObservableCollection<Scooter> Scooters { get; set; }
    
    private IMediator _mediator;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly INavigationService _navigationService;
    
    private System.Timers.Timer? _timer;

    public MainViewModel(IMediator mediator, 
        ICurrentUserProvider currentUserProvider, 
        INavigationService navigationService)
    {
        _mediator = mediator;
        _currentUserProvider = currentUserProvider;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task Appearing()
    {
        await FetchUser();
        await FetchScooters();
        await FetchReservation();
        await FetchRides();
        InitiateTimer();
    }

    [RelayCommand]
    private async Task ReservationDetails()
    {
        if (Reservation is null)
        {
            return;
        }
        var result = await Shell.Current.DisplayActionSheet("Reservation", "Cancel", null,
            "Start ride", "Cancel Reservation");
        if (result == "Cancel Reservation")
        {
            await _mediator.Send(new EndReservationCommand(Reservation.Id));
            HasReservation = false;
        } else if (result == "Start ride")
        {
            Ride ride = new Ride
            {
                ScooterId = Reservation.ScooterId,
                UserId = CurrentUserId,
                StartTime = DateTime.Now,
            };
            var response = await _mediator.Send(new CreateRideCommand(ride));
            if (response.IsSuccessful)
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "rideId", response.Data }
                };
                await _navigationService.NavigateToAsync("RidePage", parameters);
                return;
            }
            
            await Shell.Current.DisplayAlert("Error", response.ErrorMessage, "OK");
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
        await _navigationService.NavigateToAsync("//MapPage");
    }

    [RelayCommand]
    private async Task RidePageLink(Ride ride)
    {
        IDictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "rideId", ride.Id }
        };
        
        await _navigationService.NavigateToAsync("RidePage", parameters);
    }
    
    private async Task FetchScooters()
    {
        var response = await _mediator.Send(new GetAllScootersQuery());
        Scooters = response.IsSuccessful 
            ? new ObservableCollection<Scooter>(response.Data) 
            : new ObservableCollection<Scooter>();
    }

    private async Task FetchReservation()
    {
        var reservation = await _mediator.Send(new GetReservationByUserQuery(CurrentUserId));
        if (!reservation.IsSuccessful)
        {
            HasReservation = false;
        }
        else
        {
            HasReservation = true;
            Reservation = reservation.Data;
        }
    }

    private async Task FetchRides()
    {
        var response = await _mediator.Send(new GetRidesQuery(
            r => r.UserId == CurrentUserId && r.IsActive));
        if (!response.IsSuccessful)
        {
            return;
        }
        Rides = response.Data;
        if (Rides is not null || Rides.Any())
        {
            HasRides = true;
        }
    }

    private async Task FetchUser()
    {
        if (_currentUserProvider.GetCurrentUser() is not Guid userId)
        {
            await _navigationService.NavigateToAsync("//LoginPage");
            return;
        }
        CurrentUserId = userId;
    }

    private void UpdateCountdown()
    {
        if (Reservation is null)
        {
            return;
        }
        
        var startTime = Reservation.StartTime;
        var duration = Reservation.Duration;
        var endTime = startTime.AddMinutes(duration);
        var timeRemaining = endTime - DateTime.Now;

        if (timeRemaining.TotalSeconds <= 0)
        {
            _mediator.Send(new EndReservationCommand(Reservation.Id));
            Reservation = null;
            HasReservation = false;
            _timer.Stop();
            return;
        }
        
        Countdown = $"{timeRemaining.Minutes:D2}:{timeRemaining.Seconds:D2}";
    }
    
    private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(UpdateCountdown);
    }

    private void InitiateTimer()
    {
        if (_timer is not null)
        {
            _timer.Stop();
            _timer.Dispose();
        }

        _timer = new(1000);
        _timer.Elapsed += OnTimerElapsed;
        _timer.Start();
        
        UpdateCountdown();
    }
}