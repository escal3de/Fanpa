using Fanpa.Application.Contracts.Auth;
using Fanpa.Application.Handlers.Auth;
using Fanpa.Application.Handlers.Defaults;
using Fanpa.Application.Validators.Auth;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Fanpa.Application.DI;

public static class Injection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // validators
        services.AddScoped<IValidator<RegisterUserRequest>, RegisterUserRequestValidator>();
        services.AddScoped<IValidator<LoginUserRequest>, LoginUserRequestValidator>();
        
        // handlers (auth)
        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<LoginUserHandler>();
        services.AddScoped<GetMeHandler>();
        
        // handlers (defaults)
        services.AddScoped<GetUserHandler>();
        services.AddScoped<GetUsersHandler>();
        
        return services;
    }
}