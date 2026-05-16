using Fanpa.Application.Contracts.Auth;
using Fanpa.Application.Handlers.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Fanpa.API.Endpoints;

public static class AuthEndpoint
{
    public static IEndpointRouteBuilder MapAuthEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/auth");

        group.MapPost("/register",
            async (RegisterUserRequest request, [FromServices] RegisterUserHandler handler,
                CancellationToken cancellation = default) =>
            {
                var result = await handler.HandleAsync(request, cancellation);

                return result.IsSuccess
                    ? Results.Ok()
                    : Results.BadRequest(result.Error);
            });

        group.MapPost("/login",
            async (LoginUserRequest request, HttpContext context, [FromServices] LoginUserHandler handler,
                CancellationToken cancellation) =>
            {
                var result = await handler.HandleAsync(request, cancellation);

                context.Response.Cookies.Append("accessToken", result.Value.Token, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddHours(12)
                });

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest(result.Error);
            });

        group.MapGet("/logout", (HttpContext context) =>
        {
            context.Response.Cookies.Delete("accessToken", new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddHours(12)
            });
            
            return Results.NoContent();
        });

        return group;
    }
}
