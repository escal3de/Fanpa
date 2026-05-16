using System.Text.Json;
using Fanpa.Application.Abstractions.Repositories;
using Fanpa.Domain;
using Fanpa.Persistence.Realisations.Repositories;
using Microsoft.Extensions.Caching.Distributed;

namespace Fanpa.Infrastructure.Realisations.Caching;

public class CachedUsersRepository(
    UsersRepository repository,
    IDistributedCache cache) : IUsersRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly UsersRepository _repository = repository;
    private readonly IDistributedCache _cache = cache;

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _repository.AddAsync(user, cancellationToken);
        await RemoveUserKeysAsync(user.Id, user.Email, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var key = $"user:id:{id}";

        var cached = await GetAsync<User>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var user = await _repository.GetByIdAsync(id, cancellationToken);
        if (user is not null)
            await SetAsync(key, user, TimeSpan.FromMinutes(10), cancellationToken);

        return user;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        var key = $"user:email:{normalizedEmail}";
        
        var cached = await GetAsync<User>(key, cancellationToken);
        if (cached is not null)
            return cached;
        
        var user = await _repository.GetByEmailAsync(email, cancellationToken);
        if (user is not null)
        {
            await SetAsync(key, user, TimeSpan.FromMinutes(10), cancellationToken);
            await SetAsync($"user:id:{user.Id}", user, TimeSpan.FromMinutes(10), cancellationToken);
        }

        return user;
    }

    public async Task<IEnumerable<User>?> GetAllAsync(CancellationToken cancellationToken)
    {
        var key = "users:all";
        
        var cached = await GetAsync<List<User>>(key, cancellationToken);
        if (cached is not null)
            return cached;
        
        var users = await _repository.GetAllAsync(cancellationToken);
        var list = users.ToList();
        
        await SetAsync(key, list, TimeSpan.FromMinutes(2), cancellationToken);
        
        return list;
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("потом доделаю");
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(id, cancellationToken);

        await _repository.DeleteAsync(id, cancellationToken);

        if (user is not null)
            await RemoveUserKeysAsync(user.Id, user.Email, cancellationToken);
    }

    // extensions
    private async Task RemoveUserKeysAsync(Guid id, string email, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync($"user:id:{id}", cancellationToken);
        await _cache.RemoveAsync($"user:email:{email.Trim().ToLowerInvariant()}", cancellationToken);
        await _cache.RemoveAsync($"users:all", cancellationToken);
    }

    private async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
    {
        var bytes = await _cache.GetAsync(key, cancellationToken);
        if (bytes is null)
            return default;

        return JsonSerializer.Deserialize<T>(bytes, JsonOptions);
    }

    private async Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken cancellationToken)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, JsonOptions);

        await _cache.SetAsync(key, bytes, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl
        }, cancellationToken);
    }
}