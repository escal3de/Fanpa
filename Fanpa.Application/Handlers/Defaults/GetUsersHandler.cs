using CSharpFunctionalExtensions;
using Fanpa.Application.Abstractions.Repositories;
using Fanpa.Application.Contracts.Defaults;
using Fanpa.Application.Mappers;

namespace Fanpa.Application.Handlers.Defaults;

public class GetUsersHandler(IUsersRepository repository)
{
    private readonly IUsersRepository _repository = repository;

    public async Task<Result<IEnumerable<UserResponse>>> HandleAsync(CancellationToken cancellationToken)
    {
        var users = await _repository.GetAllAsync(cancellationToken);

        if (users is null)
            return Result.Failure<IEnumerable<UserResponse>>("Users list is empty");

        return Result.Success<IEnumerable<UserResponse>>(users.Select(u => u.ToResponse()).ToList());
    }
}