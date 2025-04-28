using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Scooters.ViewModels;

namespace Scooters.Views;

public partial class RidePage : Popup
{
    private RideViewModel _viewModel;
    
    public RidePage(RideViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    private async void RidePage_OnOpened(object? sender, PopupOpenedEventArgs e)
    {
        await _viewModel.AppearingCommand.ExecuteAsync(null);
    }
}