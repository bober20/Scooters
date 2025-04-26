using Application.Interfaces;
using Infrastructure.Authentication.JwtTokenGenerator;
using Infrastructure.Authentication.PasswordHasher;
using Infrastructure.Common.Persistence;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ScootersDbContext>();
        
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IRideRepository, RideRepository>();
        services.AddTransient<IScooterRepository, ScooterRepository>();
        services.AddTransient<IReservationRepository, ReservationRepository>();

        services.AddTransient<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ScootersDbContext>());
        
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));
        
        services.AddTransient<IPasswordHasher, PasswordHasher>();
        services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddTransient<IJwtTokenValidator, JwtTokenValidator>();
        
        return services;
    }
}