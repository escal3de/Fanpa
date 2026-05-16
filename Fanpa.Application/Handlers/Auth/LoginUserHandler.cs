using CSharpFunctionalExtensions;
using Fanpa.Application.Abstractions.Repositories;
using Fanpa.Application.Abstractions.Security;
using Fanpa.Application.Contracts.Auth;
using FluentValidation;

namespace Fanpa.Application.Handlers.Auth;

public class LoginUserHandler(
    IUsersRepository repository,
    IValidator<LoginUserRequest> validator,
    IJwtProvider jwtProvider,
    IPasswordHasher passwordHasher)
{
    private readonly IUsersRepository _repository = repository;
    private readonly IValidator<LoginUserRequest> _validator = validator;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<Result<LoginResponse>> HandleAsync(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Result.Failure<LoginResponse>(
                string.Join(string.Empty, validationResult.Errors.Select(e => e.ErrorMessage)));

        var user = await _repository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
            return Result.Failure<LoginResponse>("Invalid credentials");

        var verifyPassword = _passwordHasher.Verify(user.HashedPassword, request.Password);

        if (!verifyPassword)
            return Result.Failure<LoginResponse>("Invalid credentials");

        var token = _jwtProvider.GenerateJwt(user);
        
        return Result.Success(new LoginResponse(token, user.Name, user.Email, user.CreatedAt));
    }
}