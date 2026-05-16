namespace Fanpa.Application.Contracts.Auth;

public record RegisterUserRequest(
    string Name,
    string Email,
    string Password);