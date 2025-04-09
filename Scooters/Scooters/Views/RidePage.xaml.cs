using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scooters.ViewModels;

namespace Scooters.Views;

public partial class RidePage : ContentPage
{
    public RidePage(RideViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}