using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<UserService>();
        services.AddTransient<RideService>();
        services.AddTransient<ReservationService>();
        services.AddTransient<ScooterService>();
        
        return services;
    }
}