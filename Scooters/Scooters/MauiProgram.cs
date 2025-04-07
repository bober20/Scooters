using Application.Common.Interfaces.CurrentUserProvider;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Maps;
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
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("arial.ttf", "Arial");
                fonts.AddFont("MauiMaterialAssets.ttf", "MauiMaterialAssets");
            })
            .UseMauiMaps()
            .UseBarcodeReader();

        var appSettingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        builder.Configuration.AddJsonFile(appSettingsPath, optional: false);

        builder.Services
            .AddApplication()
            .AddInfrastructure(builder.Configuration);

        builder.Services.AddTransient<ICurrentUserProvider, CurrentUserProvider>();

        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MainPage>();

        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();

        builder.Services.AddTransient<SignUpViewModel>();
        builder.Services.AddTransient<SignUpPage>();

        builder.Services.AddTransient<ProfileViewModel>();
        builder.Services.AddTransient<ProfilePage>();

        builder.Services.AddTransient<ScootersMapViewModel>();
        builder.Services.AddTransient<ScootersMapPage>();

        builder.Services.AddTransient<ReservationViewModel>();
        builder.Services.AddTransient<ReservationPage>();

        builder.Services.AddTransient<PasswordChangeViewModel>();
        builder.Services.AddTransient<PasswordChangePage>();

        builder.Services.AddTransient<RideViewModel>();
        builder.Services.AddTransient<RideView>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}