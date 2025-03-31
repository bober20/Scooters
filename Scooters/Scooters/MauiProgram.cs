using Application.Common.Interfaces.CurrentUserProvider;
using Infrastructure.Authentication.JwtTokenGenerator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Scooters.Services;
using Scooters.ViewModels;
using Scooters.Views;

namespace Scooters;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        
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

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}