using Fanpa.Domain.Additional;

namespace Fanpa.Application.Contracts.Defaults;

public record UserResponse(
    Guid Id,
    string Name,
    string Email,
    DateTime CreatedAt,
    double Rating,
    List<Review> Reviews);