using Fanpa.Domain;
using Fanpa.Domain.Additional;
using Fanpa.Persistence.Entities;

namespace Fanpa.Persistence.Mapper;

public static class UsersMapper
{
    public static UserEntity ToEntity(this User user)
        => new UserEntity
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            HashedPassword = user.HashedPassword,
            CreatedAt = user.CreatedAt,
            Rating = user.Rating,
            Roles = user.Roles,
            Reviews = user.Reviews.Select(review => new ReviewEntity
            {
                Id = review.Id,
                Theme = review.Theme,
                Description = review.Description,
                Money = review.Money,
                Rating = review.Rating,
                GameName = review.GameName,
                SellerAnswer = review.SellerAnswer,
                CreatedAt = review.CreatedAt
            }).ToList()
        };

    public static User ToDomain(this UserEntity entity)
        => User.Load(
            entity.Id,
            entity.Name,
            entity.Email,
            entity.HashedPassword,
            entity.CreatedAt,
            entity.Rating,
            entity.Roles,
            entity.Reviews.Select(review => Review.Load(
                review.Id,
                review.Theme,
                review.Description,
                review.Money,
                review.Rating,
                review.GameName,
                review.SellerAnswer,
                review.CreatedAt)).ToList());
}
