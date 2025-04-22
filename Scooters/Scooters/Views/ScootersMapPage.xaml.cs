using Scooters.ViewModels;

namespace Scooters.Views;

public partial class ScootersMapPage : ContentPage
{
    private readonly ScootersMapViewModel _viewModel;
    
    public ScootersMapPage(ScootersMapViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    private void OnAppearing(object sender, EventArgs e)
    {
        _viewModel.AppearingCommand.Execute(null);
        
        ScootersMap.Pins.Clear();
        foreach (var pin in _viewModel.Pins)
        {
            ScootersMap.Pins.Add(pin);
        }
    }
}