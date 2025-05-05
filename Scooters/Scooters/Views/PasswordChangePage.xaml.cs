using Scooters.ViewModels;

namespace Scooters.Views;

public partial class PasswordChangePage : ContentPage
{
    public PasswordChangePage(PasswordChangeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}