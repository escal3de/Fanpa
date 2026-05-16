using System.Text;
using CSharpFunctionalExtensions;

namespace Fanpa.Domain.Additional;

public class Review
{
    public Guid Id { get; private set; }
    public string Theme { get; private set; }
    public string Description { get; private set; }
    public decimal Money { get; private set; }
    public int Rating { get; private set; }
    public string GameName { get; private set; }
    public string SellerAnswer { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Review(Guid id, string theme, string description, decimal money, int rating, string gameName,
        string sellerAnswer, DateTime createdAt)
    {
        Id = id;
        Theme = theme;
        Description = description;
        Money = money;
        Rating = rating;
        GameName = gameName;
        SellerAnswer = sellerAnswer;
        CreatedAt = createdAt;
    }

    public static Result<Review> Create(string theme, string description, decimal money, int rating, string gameName)
    {
        var validationResult = Validate(theme, description, money, rating, gameName);

        if (validationResult.IsFailure)
            return Result.Failure<Review>(validationResult.Error);

        var review = new Review(Guid.NewGuid(), theme, description, money, rating, gameName, string.Empty,
            DateTime.UtcNow);
        
        return Result.Success(review);
    }

    public Review Load() => new Review(Id, Theme, Description, Money, Rating, GameName, SellerAnswer, CreatedAt);

    public static Review Load(Guid id, string theme, string description, decimal money, int rating, string gameName,
        string sellerAnswer, DateTime createdAt)
        => new Review(id, theme, description, money, rating, gameName, sellerAnswer, createdAt);

    private static Result Validate(string theme, string description, decimal money, int rating, string gameName)
    {
        if (string.IsNullOrWhiteSpace(theme) || (theme.Length > 32 || theme.Length < 10))
            return Result.Failure("Invalid theme");

        if (string.IsNullOrWhiteSpace(description) || (description.Length > 2056 || description.Length < 32))
            return Result.Failure("Invalid description");

        if (money <= 0)
            return Result.Failure("Invalid amount of money");

        if (rating > 5 || rating < 1)
            return Result.Failure("Invalid rating");

        if (string.IsNullOrWhiteSpace(gameName) || (gameName.Length > 128 || gameName.Length < 1))
            return Result.Failure("Invalid name of game");
        
        return Result.Success();
    }
}
