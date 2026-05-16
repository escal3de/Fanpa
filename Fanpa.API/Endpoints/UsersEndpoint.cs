using System.Security.Claims;
using Fanpa.Application.Handlers.Auth;
using Fanpa.Application.Handlers.Defaults;
using Microsoft.AspNetCore.Mvc;

namespace Fanpa.API.Endpoints;

public static class UsersEndpoint
{
    public static IEndpointRouteBuilder MapUsersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/users");

        group.MapGet("/me",
            async (ClaimsPrincipal user, [FromServices] GetMeHandler handler, CancellationToken cancellationToken) =>
            {
                var userId = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                if (!Guid.TryParse(userId, out var id))
                    return Results.BadRequest();

                var result = await handler.HandleAsync(id, cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.NotFound(result.Error);
            }).RequireAuthorization("ForClient");

        group.MapGet("/", async ([FromServices] GetUsersHandler handler, CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(result.Error);
        }).RequireAuthorization("ForClient");

        group.MapGet("/{value}",
            async (string value, [FromServices] GetUserHandler handler, CancellationToken cancellationToken) =>
            {
                var result = await handler.HandleAsync(value, cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.NotFound(result.Error);
            }).RequireAuthorization("ForClient");

        return group;
    }
}