using Fanpa.Application.Abstractions.Repositories;
using Fanpa.Domain;
using Fanpa.Persistence.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Fanpa.Persistence.Realisations.Repositories;

public class UsersRepository(UsersDbContext dbContext) : IUsersRepository
{
    private readonly UsersDbContext _dbContext = dbContext;

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user.ToEntity(), cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        return user?.ToDomain();
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        return user?.ToDomain();
    }

    public async Task<IEnumerable<User>?> GetAllAsync(CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return user.Select(u => u.ToDomain());
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("апдейт нужно доделать потом");
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _dbContext.Users
            .Where(u => u.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}