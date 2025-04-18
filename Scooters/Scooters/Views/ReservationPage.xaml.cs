using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Scooters.ViewModels;

namespace Scooters.Views;

public partial class ReservationPage : Popup
{
    private ReservationViewModel _viewModel;
    
    public ReservationPage(ReservationViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    private void ReservationPage_OnOpened(object? sender, PopupOpenedEventArgs e)
    {
        _viewModel.AppearingCommand.Execute(null);
    }
}