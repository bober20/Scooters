using Scooters.Views;

namespace Scooters;

public partial class AppShell : Shell
{
    public AppShell()
    {
        Routing.RegisterRoute(nameof(PasswordChangePage), typeof(PasswordChangePage));
        InitializeComponent();
    }
}