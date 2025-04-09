using Application.Common.Interfaces.CurrentUserProvider;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Scooters.Services;
using Scooters.ViewModels;
using Scooters.Views;
using ZXing.Net.Maui.Controls;

namespace Scooters;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiMaps()
            .UseBarcodeReader();

        var appSettingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        builder.Configuration.AddJsonFile(appSettingsPath, optional: false);

        builder.Services
            .AddApplication()
            .AddInfrastructure(builder.Configuration);

        builder.Services.AddTransient<ICurrentUserProvider, CurrentUserProvider>();

        builder.Services.AddTransient<MainPage, MainViewModel>();
        builder.Services.AddTransient<LoginPage, LoginViewModel>();
        builder.Services.AddTransient<SignUpPage, SignUpViewModel>();
        builder.Services.AddTransient<ProfilePage, ProfileViewModel>();
        builder.Services.AddTransient<ScootersMapPage, ScootersMapViewModel>();
        
        builder.Services.AddTransientWithShellRoute<ReservationPage, ReservationViewModel>("ReservationPage");
        builder.Services.AddTransientWithShellRoute<PasswordChangePage, PasswordChangeViewModel>("PasswordChangePage");
        builder.Services.AddTransientWithShellRoute<RidePage, RideViewModel>("RidePage");
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}