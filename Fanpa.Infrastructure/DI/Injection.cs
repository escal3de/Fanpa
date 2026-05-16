using Fanpa.Application.Abstractions.Security;
using Fanpa.Domain;
using Fanpa.Infrastructure.Realisations.Security;
using Fanpa.Infrastructure.Realisations.Security.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fanpa.Infrastructure.DI;

public static class Injection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var expiresHoursValue = configuration["JwtOptions:ExpiresHours"] ?? configuration["JwtOptions:ExpiresIn"];
        var expiresHours = int.TryParse(expiresHoursValue, out var parsedExpiresHours) ? parsedExpiresHours : 12;

        var jwtOptions = new JwtOptions
        {
            SecretKey = configuration["JwtOptions:SecretKey"] ?? string.Empty,
            ExpiresHours = expiresHours
        };

        if (string.IsNullOrWhiteSpace(jwtOptions.SecretKey))
            throw new ArgumentException("Missing JwtOptions:SecretKey configuration value.");

        services.AddSingleton(Options.Create(jwtOptions));

        services.AddScoped<Microsoft.AspNetCore.Identity.IPasswordHasher<User>,
                Microsoft.AspNetCore.Identity.PasswordHasher<User>>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });

        return services;
    }
}
