using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scooters.ViewModels;

namespace Scooters.Views;

public partial class SignUpPage : ContentPage
{
    public SignUpPage(SignUpViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}