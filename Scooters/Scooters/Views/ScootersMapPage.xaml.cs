using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scooters.ViewModels;

namespace Scooters.Views;

public partial class ScootersMapPage : ContentPage
{
    public ScootersMapPage(ScootersMapViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        foreach (var scooter in viewModel.Pins)
        {
            ScootersMap.Pins.Add(scooter);
        }
    }
}