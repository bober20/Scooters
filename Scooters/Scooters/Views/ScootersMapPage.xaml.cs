using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Scooters.Common.Services;
using Scooters.ViewModels;

namespace Scooters.Views;

public partial class ScootersMapPage : ContentPage, IMapUpdaterService
{
    public ScootersMapPage(ScootersMapViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        
        viewModel.SetMapService(this);
    }

    public void SetPins(IEnumerable<Pin> pins)
    {
        ScootersMap.Pins.Clear();
        foreach (var pin in pins)
            ScootersMap.Pins.Add(pin);
    }

    public void MoveTo(Location center, double latSpan = 0.0001, double lonSpan = 0.0001)
    {
        ScootersMap.MoveToRegion(MapSpan.FromCenterAndRadius(center, Distance.FromKilometers(latSpan)));
    }
}

// public partial class ScootersMapPage : ContentPage
// {
//     private readonly ScootersMapViewModel _viewModel;
//     
//     public ScootersMapPage(ScootersMapViewModel viewModel)
//     {
//         InitializeComponent();
//         BindingContext = viewModel;
//         _viewModel = viewModel;
//     }
//
     // private async void OnAppearing(object sender, EventArgs e)
     // {
     //     // await _viewModel.AppearingCommand.ExecuteAsync(null);
     //     // UpdatePins();
     // }
//
//     private async void OnPinTapped(object sender, PinClickedEventArgs e)
//     {
//         e.HideInfoWindow = true;
//         await _viewModel.PinClickedCommand.ExecuteAsync(sender);
//         UpdatePins();
//     }
//
//     private void UpdatePins()
//     {
//         ScootersMap.Pins.Clear();
//         foreach (var pin in _viewModel.Pins)
//         {
//             pin.MarkerClicked += OnPinTapped;
//             ScootersMap.Pins.Add(pin);
//         }
//     }
// }