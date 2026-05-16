namespace Fanpa.Application.Contracts.Auth;

public record LoginUserRequest(
    string Email,
    string Password);