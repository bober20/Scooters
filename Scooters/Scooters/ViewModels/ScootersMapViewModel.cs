using System.Collections.ObjectModel;
using Application.Common.Interfaces;
using Application.Reservations.Commands.EndReservation;
using Application.Reservations.Queries.GetReservationByUser;
using Application.Rides.Commands.CreateRide;
using Application.Rides.Queries.GetRideByUserId;
using Application.Scooters.Queries.GetAllScooters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using MediatR;
using Microsoft.Maui.Controls.Maps;
using Scooters.Common.Interfaces;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace Scooters.ViewModels;

public partial class ScootersMapViewModel : ObservableObject
{
    [ObservableProperty] private List<Scooter> _scooters;
    [ObservableProperty] private Scooter _selectedScooter;
    [ObservableProperty] private ObservableCollection<Pin> _pins = new();
    [ObservableProperty] private bool _hasReservation;
    [ObservableProperty] private bool _hasRide;
    [ObservableProperty] private string _countdown;
    [ObservableProperty] private Guid _currentUserId;
    [ObservableProperty] private Reservation? _reservation;
    [ObservableProperty] private Ride? _ride;
    [ObservableProperty] private Map _map;

    private IBottomSheetService _bottomSheetService;
    private readonly IMediator _mediator;
    private readonly INavigationService _navigationService;
    private readonly ICurrentUserProvider _currentUserProvider;
    private IMapUpdaterService _mapUpdaterService;

    private System.Timers.Timer? _timer;

    public ScootersMapViewModel(IMediator mediator, INavigationService navigationService,
        ICurrentUserProvider currentUserProvider)
    {
        _mediator = mediator;
        _navigationService = navigationService;
        _currentUserProvider = currentUserProvider;
    }

    [RelayCommand]
    private void ShowAppManual()
    {
        _bottomSheetService.ShowBottomSheetCall();
    }

    [RelayCommand]
    private async Task Appearing()
    {
        await FetchUser();
        await Task.WhenAll(FetchReservation(), FetchRide(), FetchScooters());
        if (Reservation is not null)
        {
            InitiateTimer();
        }

        AddPins();
    }

    [RelayCommand]
    private async Task RidePageLink()
    {
        await _navigationService.NavigateToRidePageAsync(Ride);

        await Task.WhenAll(FetchReservation(), FetchRide(), FetchScooters());
        AddPins();
    }

    [RelayCommand]
    private async Task ScooterActions()
    {
        var result = await Shell.Current.DisplayActionSheet("Reservation", "Cancel",
            "Cancel Reservation", "Start ride");

        if (result == "Cancel Reservation")
        {
            await EndReservation();
        }
        else if (result == "Start ride")
        {
            await CreateRide();
            await FetchRide();
        }

        await FetchScooters();
        AddPins();
    }

    public void SetBottomSheetService(IBottomSheetService bottomSheetService)
    {
        _bottomSheetService = bottomSheetService;
    }

    private async Task EndReservation()
    {
        if (Reservation is null) return;

        await _mediator.Send(new EndReservationCommand(Reservation.Id));

        Reservation = null;
        HasReservation = false;
        _timer?.Stop();
        _timer?.Dispose();
    }

    private async Task CreateRide()
    {
        Ride ride = new Ride
        {
            ScooterId = Reservation.ScooterId,
            UserId = CurrentUserId,
            StartTime = DateTime.Now,
        };

        var response = await _mediator.Send(new CreateRideCommand(ride));
        if (!response.IsSuccessful)
        {
            await Shell.Current.DisplayAlert("Error", response.ErrorMessage, "OK");
            return;
        }

        Reservation = null;
        HasReservation = false;
        _timer?.Stop();
        _timer?.Dispose();

        await _navigationService.NavigateToRidePageAsync(ride);
    }

    private void AddPins()
    {
        Pins.Clear();

        foreach (var s in Scooters)
        {
            var pin = new Pin
            {
                Label = s.ModelDescription,
                Type = PinType.SavedPin,
                Location = new Location(s.Coordinates.Latitude, s.Coordinates.Longitude),
                BindingContext = s
            };
            Pins.Add(pin);
            pin.MarkerClicked += OnPinMarkerClicked;
        }

        if (Reservation?.Scooter is not null)
        {
            var pin = new Pin
            {
                Label = "Current reservation",
                Type = PinType.SavedPin,
                Location = new Location(Reservation.Scooter.Coordinates.Latitude,
                    Reservation.Scooter.Coordinates.Longitude),
                BindingContext = Reservation.Scooter,
            };
            pin.MarkerClicked += OnPinMarkerClicked;
            _mapUpdaterService.MoveTo(new Location(Reservation.Scooter.Coordinates.Latitude,
                Reservation.Scooter.Coordinates.Longitude));
            Pins.Add(pin);
        }

        if (Ride?.Scooter is not null)
        {
            var pin = new Pin
            {
                Label = "Current ride",
                Type = PinType.SavedPin,
                Location = new Location(Ride.Scooter.Coordinates.Latitude, Ride.Scooter.Coordinates.Longitude),
                BindingContext = Ride.Scooter
            };
            pin.MarkerClicked += OnPinMarkerClicked;
            _mapUpdaterService.MoveTo(new Location(Ride.Scooter.Coordinates.Latitude,
                Ride.Scooter.Coordinates.Longitude));
            Pins.Add(pin);
        }

        _mapUpdaterService.SetPins(Pins);
    }

    private async void OnPinMarkerClicked(object sender, PinClickedEventArgs e)
    {
        e.HideInfoWindow = true;

        if (sender is Pin pin && pin.BindingContext is Scooter scooter)
        {
            await _navigationService.NavigateToReservationPageAsync(scooter);
            await Task.WhenAll(FetchReservation(), FetchRide(), FetchScooters());
            AddPins();
        }
    }

    public void SetMapService(IMapUpdaterService mapUpdaterService)
    {
        _mapUpdaterService = mapUpdaterService;
    }

    private async Task FetchReservation()
    {
        var reservation = await _mediator.Send(new GetReservationByUserQuery(CurrentUserId));
        if (reservation.IsSuccessful)
        {
            HasReservation = true;
            Reservation = reservation.Data;
            InitiateTimer();
            return;
        }

        Ride = null;
        HasReservation = false;
    }

    private async Task FetchRide()
    {
        var response = await _mediator.Send(new GetRideQuery(CurrentUserId));

        if (response.IsSuccessful)
        {
            HasRide = true;
            Ride = response.Data;
            return;
        }

        Ride = null;
        HasRide = false;
    }

    private async Task FetchUser()
    {
        if (_currentUserProvider.GetCurrentUser() is not Guid userId)
        {
            await _navigationService.NavigateToLoginPageAsync();
            return;
        }

        CurrentUserId = userId;
    }

    private async Task FetchScooters()
    {
        var response = await _mediator.Send(new GetAvailableScootersQuery());
        Scooters = new List<Scooter>(response.Data);
    }

    private void UpdateCountdown()
    {
        var startTime = Reservation.StartTime;
        var duration = Reservation.Duration;
        var endTime = startTime.AddMinutes(duration);
        var timeRemaining = endTime - DateTime.Now;

        if (timeRemaining.TotalSeconds <= 0)
        {
            _mediator.Send(new EndReservationCommand(Reservation.Id));
            Reservation = null;
            HasReservation = false;
            _timer?.Stop();
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