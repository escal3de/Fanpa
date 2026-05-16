using System.Text;
using Fanpa.Infrastructure.Realisations.Security.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Fanpa.API.ServiceCollections;

public static class CustomAuthentication
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtOptions = configuration.GetRequiredSection("JwtOptions").Get<JwtOptions>() ??
                         throw new ArgumentException("Missing configuration options.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };

                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["accessToken"];

                        if (!string.IsNullOrWhiteSpace(token))
                            context.Token = token;

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("ForClient", policy => policy.RequireRole("Client"));
            options.AddPolicy("ForSupport", policy => policy.RequireRole("Support"));
            options.AddPolicy("ForAdmin", policy => policy.RequireRole("Admin"));
        });

        return services;
    }
}