using Application.Common.Interfaces.CurrentUserProvider;
using Application.Common.Interfaces.NavigationService;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
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
            .UseBarcodeReader()
            .UseLocalNotification();

        var appSettingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        builder.Configuration.AddJsonFile(appSettingsPath, optional: false);

        builder.Services
            .AddApplication()
            .AddInfrastructure(builder.Configuration);

        builder.Services.AddSingleton<ICurrentUserProvider, CurrentUserProvider>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        
        builder.Services.AddTransient<MainPage, MainViewModel>();
        builder.Services.AddTransient<QRScannerPage, QRScannerViewModel>();
        builder.Services.AddTransient<LoginPage, LoginViewModel>();
        builder.Services.AddTransient<SignUpPage, SignUpViewModel>();
        builder.Services.AddTransient<ProfilePage, ProfileViewModel>();
        builder.Services.AddTransient<ScootersMapPage, ScootersMapViewModel>();
        
        builder.Services.AddTransientPopup<ReservationPage, ReservationViewModel>();
        builder.Services.AddTransientPopup<RidePage, RideViewModel>();
        builder.Services.AddTransientWithShellRoute<PasswordChangePage, PasswordChangeViewModel>("PasswordChangePage");
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}