namespace Fanpa.Application.Contracts.Auth;

public record LoginResponse(
    string Token,
    string Name,
    string Email,
    DateTime CreatedAt);