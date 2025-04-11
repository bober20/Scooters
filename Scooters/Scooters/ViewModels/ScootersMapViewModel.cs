using Application.Common.Interfaces.NavigationService;
using Application.Scooters.Queries.GetAllScooters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using MediatR;
using Microsoft.Maui.Controls.Maps;
using Scooters.Views;

namespace Scooters.ViewModels;

public partial class ScootersMapViewModel : ObservableObject
{
    [ObservableProperty] private List<Scooter> _scooters;
    [ObservableProperty] private Scooter _selectedScooter;
    [ObservableProperty] private List<Pin> _pins;
    
    private readonly IMediator _mediator;
    private readonly INavigationService _navigationService;
    
    public ScootersMapViewModel(IMediator mediator, INavigationService navigationService)
    {
        _mediator = mediator;
        _navigationService = navigationService;
        AddPins();
    }
    
    [RelayCommand]
    private async Task Appearing()
    {
        await LoadScooters();
    }

    [RelayCommand]
    private async Task PinClicked(Pin pin)
    {
        if (pin.BindingContext is not Scooter scooter)
        {
            return;
        }
    
        Dictionary<string, object> parameters = new()
        {
            { "scooterId", scooter.Id }
        };
        
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await _navigationService.NavigateToAsync("ReservationPage", parameters);
        });
    }
    
    private async Task AddPins()
    {
        Pins = new List<Pin>();
        var response = await _mediator.Send(new GetAllScootersQuery());
        if (!response.IsSuccessful) return;
        foreach (var s in response.Data)
        {
            var pin = new Pin
            {
                Label = s.ModelDescription,
                Type = PinType.Place,
                Location = new Location(s.Coordinates.Latitude, s.Coordinates.Longitude),
                BindingContext = s
            };
            pin.MarkerClicked += async (s, e) =>
            {
                e.HideInfoWindow = true;
                if (PinClickedCommand.CanExecute(s))
                {
                    PinClickedCommand.Execute(s);
                }
            };
            Pins.Add(pin);
        }
    }
    
    private async Task LoadScooters()
    {
        var response = await _mediator.Send(new GetAllScootersQuery());
        Scooters = new List<Scooter>(response.Data);
    }
}