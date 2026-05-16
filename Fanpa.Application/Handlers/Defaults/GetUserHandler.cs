using CSharpFunctionalExtensions;
using Fanpa.Application.Abstractions.Repositories;
using Fanpa.Application.Contracts.Defaults;
using Fanpa.Application.Mappers;

namespace Fanpa.Application.Handlers.Defaults;

public class GetUserHandler(IUsersRepository repository)
{
    private readonly IUsersRepository _repository = repository;

    public async Task<Result<UserResponse>> HandleAsync(string value, CancellationToken cancellationToken)
    {
        var user = value switch
        {
            var x when Guid.TryParse(value, out var id) => await _repository.GetByIdAsync(id, cancellationToken),
            var x when value[0] != '@' && value.Contains('@') => await _repository.GetByEmailAsync(value, cancellationToken),
            _ => null
        };

        if (user is null)
            return Result.Failure<UserResponse>("User not found");

        return Result.Success(user.ToResponse());
    }
}