using System.Collections.ObjectModel;
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
    public ObservableCollection<Scooter> Scooters { get; set; }
    private IMediator _mediator;

    public MainViewModel(IMediator mediator)
    {
        _mediator = mediator;
        FetchScooters();
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
}