namespace Fanpa.Persistence.Entities;

public class ReviewEntity
{
    public Guid Id { get; set; }
    public string Theme { get;  set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Money { get; set; }
    public int Rating { get; set; }
    public string GameName { get; set; } = string.Empty;
    public string SellerAnswer { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;
}