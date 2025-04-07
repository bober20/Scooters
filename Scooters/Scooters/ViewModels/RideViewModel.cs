using CommunityToolkit.Mvvm.ComponentModel;
using Domain.Entities;

namespace Scooters.ViewModels;

public partial class RideViewModel : ObservableObject
{
    [ObservableProperty] private Ride _ride;
    [ObservableProperty] private Scooter _scooter;
}