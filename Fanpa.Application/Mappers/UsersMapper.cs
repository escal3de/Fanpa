using Fanpa.Application.Contracts.Defaults;
using Fanpa.Domain;

namespace Fanpa.Application.Mappers;

public static class UsersMapper
{
    public static UserResponse ToResponse(this User user)
        => new UserResponse(user.Id, user.Name, user.Email, user.CreatedAt, user.Rating, user.Reviews);
}