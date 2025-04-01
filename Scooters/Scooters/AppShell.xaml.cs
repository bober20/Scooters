using Scooters.Views;

namespace Scooters;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(PasswordChangePage), typeof(PasswordChangePage));
        Routing.RegisterRoute(nameof(ReservationPage), typeof(ReservationPage));
    }
}