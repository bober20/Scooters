using Application.Common.Interfaces.CurrentUserProvider;
using Application.Common.Interfaces.NavigationService;
using Application.Reservations.Commands.EndReservation;
using Application.Reservations.Queries.GetReservationByUser;
using Application.Rides.Commands.CreateRide;
using Application.Rides.Queries.GetRidesByFilter;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using MediatR;

namespace Scooters.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private Guid _currentUserId;
    [ObservableProperty] private Reservation? _reservation;
    [ObservableProperty] private List<Ride> _rides;
    [ObservableProperty] private bool _hasReservation;
    [ObservableProperty] private bool _hasRides;
    [ObservableProperty] private string _countdown;

    private readonly IMediator _mediator;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly INavigationService _navigationService;

    private System.Timers.Timer? _timer;

    public MainViewModel(IMediator mediator, ICurrentUserProvider currentUserProvider,
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
        Task.WaitAll(FetchReservation(), FetchRides());
        InitiateTimer();
    }

    [RelayCommand]
    private async Task ScooterActions()
    {
        if (Reservation is null)
        {
            return;
        }

        var result = await _navigationService.ShowOptionsAsync("Reservation", "Cancel", 
            "Start ride", "Cancel Reservation");
        if (result == "Cancel Reservation")
        {
            await _mediator.Send(new EndReservationCommand(Reservation.Id));
            HasReservation = false;
        }
        else if (result == "Start ride")
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
                await _navigationService.ShowPopupAsync<RideViewModel>(
                    onPresenting: viewModel => viewModel.RideId = response.Data);
                Task.WaitAll(FetchReservation(), FetchRides());
                return;
            }
            
            await _navigationService.ShowAlertAsync("Error", response.ErrorMessage);
        }
    }

    [RelayCommand]
    private async Task MapPageLink() => await _navigationService.NavigateToMapPageAsync();

    [RelayCommand]
    private async Task RidePageLink(Ride ride)
    {
        await _navigationService.ShowPopupAsync<RideViewModel>(
            onPresenting: viewModel => viewModel.RideId = ride.Id);
        Task.WaitAll(FetchReservation(), FetchRides());
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
        var response = await _mediator.Send(new GetRidesQuery(r => r.UserId == CurrentUserId && r.IsActive));
        Rides = response.Data;
        HasRides = Rides?.Count > 0;
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