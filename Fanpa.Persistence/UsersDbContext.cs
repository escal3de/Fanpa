using Fanpa.Persistence.Configurations;
using Fanpa.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fanpa.Persistence;

public class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<ReviewEntity> Reviews => Set<ReviewEntity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new ReviewConfiguration());
        
        base.OnModelCreating(builder);
    }
}