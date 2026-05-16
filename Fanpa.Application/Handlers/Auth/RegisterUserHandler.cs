using CSharpFunctionalExtensions;
using Fanpa.Application.Abstractions.Repositories;
using Fanpa.Application.Abstractions.Security;
using Fanpa.Application.Contracts.Auth;
using Fanpa.Domain;
using FluentValidation;

namespace Fanpa.Application.Handlers.Auth;

public class RegisterUserHandler(
    IUsersRepository repository,
    IValidator<RegisterUserRequest> validator,
    IPasswordHasher passwordHasher)
{
    private readonly IUsersRepository _repository = repository;
    private readonly IValidator<RegisterUserRequest> _validator = validator;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<Result> HandleAsync(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Result.Failure(
                string.Join(string.Empty, validationResult.Errors.Select(e => e.ErrorMessage)));

        if (await _repository.GetByEmailAsync(request.Email, cancellationToken) != null)
            return Result.Failure("This email is already exist");

        var hashedPassword = _passwordHasher.Generate(request.Password);
        
        var user = User.Create(request.Name, request.Email, hashedPassword);

        if (user.IsFailure)
            return Result.Failure(user.Error);

        await _repository.AddAsync(user.Value, cancellationToken);
        
        return Result.Success();
    }
}