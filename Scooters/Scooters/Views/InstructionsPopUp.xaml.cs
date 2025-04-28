using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using Scooters.ViewModels;

namespace Scooters.Views;

public partial class InstructionsPopUp : Popup
{
    public InstructionsPopUp(InstructionsViewModel instructionsViewModel)
    {
        InitializeComponent();
        BindingContext = instructionsViewModel;
    }
}