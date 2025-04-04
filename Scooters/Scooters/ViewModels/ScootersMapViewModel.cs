using System.Collections.ObjectModel;
using Application.Scooters.Queries.GetAllScooters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using MediatR;

namespace Scooters.ViewModels;

public partial class ScootersMapViewModel : ObservableObject
{
    public ObservableCollection<Scooter> Scooters { get; set; }
    [ObservableProperty] private Scooter _selectedScooter;
    private readonly IMediator _mediator;
    
    public ScootersMapViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }
    private async Task LoadScooters()
    {
        var response = await _mediator.Send(new GetAllScootersQuery());
        Scooters = new ObservableCollection<Scooter>(response.Data);
    }
    
    [RelayCommand]
    private async Task Appearing()
    {
        await LoadScooters();
    }
    
    [RelayCommand]
    private async Task ReservationLink()
    {
        Dictionary<string, object> parameters = new()
        {
            {"scooterId", SelectedScooter.Id}
        };
        await Shell.Current.GoToAsync($"ReservationPage", parameters);
    }
}