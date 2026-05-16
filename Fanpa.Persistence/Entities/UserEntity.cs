using Fanpa.Domain;
using Fanpa.Domain.Additional;

namespace Fanpa.Persistence.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public double Rating { get; set; }
    public List<UserRole> Roles { get; set; } = new List<UserRole>();
    public List<ReviewEntity> Reviews { get; set; } = new List<ReviewEntity>();
}
