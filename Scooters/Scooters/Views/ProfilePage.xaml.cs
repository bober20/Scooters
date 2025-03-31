using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scooters.ViewModels;

namespace Scooters.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(ProfileViewModel profile)
    {
        BindingContext = profile;
        InitializeComponent();
    }
}