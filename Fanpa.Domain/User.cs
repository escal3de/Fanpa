using CSharpFunctionalExtensions;
using Fanpa.Domain.Additional;

namespace Fanpa.Domain;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string HashedPassword { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public double Rating { get; private set; }
    public List<UserRole> Roles { get; private set; }
    public List<Review> Reviews { get; private set; }

    private User(Guid id, string name, string email, string hashedPassword, DateTime createdAt, double rating,
        List<UserRole> roles, List<Review> reviews)
    {
        Id = id;
        Name = name;
        Email = email;
        HashedPassword = hashedPassword;
        CreatedAt = createdAt;
        Rating = rating;
        Roles = roles;
        Reviews = reviews;
    }

    public static Result<User> Create(string name, string email, string hashedPassword)
    {
        var validationResult = Validate(name, email, hashedPassword, 0);

        if (validationResult.IsFailure)
            return Result.Failure<User>(validationResult.Error);

        var user = new User(Guid.NewGuid(), name, email, hashedPassword, DateTime.UtcNow, 0,
            new List<UserRole>() { UserRole.Client }, new List<Review>());

        return Result.Success(user);
    }

    public static User Load(Guid id, string name, string email, string hashedPassword, DateTime createdAt,
        double rating, List<UserRole> roles, List<Review> reviews)
        => new User(id, name, email, hashedPassword, createdAt, rating, roles, reviews);

    private static Result Validate(string name, string email, string hashedPassword, double rating)
    {
        if (string.IsNullOrWhiteSpace(name) || (name.Length > 32 || name.Length < 3))
            return Result.Failure("Invalid name");

        if (string.IsNullOrWhiteSpace(email) || (email.Length > 128 || email.Length < 1))
            return Result.Failure("Invalid email");

        if (string.IsNullOrWhiteSpace(hashedPassword))
            return Result.Failure("Empty hash");

        if (rating > 5 || rating < 0)
            return Result.Failure("Invalid rating");

        return Result.Success();
    }
}