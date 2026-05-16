using CSharpFunctionalExtensions;
using Fanpa.Application.Abstractions.Repositories;
using Fanpa.Application.Contracts.Defaults;
using Fanpa.Application.Mappers;

namespace Fanpa.Application.Handlers.Auth;

public class GetMeHandler(IUsersRepository repository)
{
    private readonly IUsersRepository _repository = repository;

    public async Task<Result<UserResponse>> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(id, cancellationToken);

        if (user is null)
            return Result.Failure<UserResponse>("User not found");

        return Result.Success(user.ToResponse());
    }
}