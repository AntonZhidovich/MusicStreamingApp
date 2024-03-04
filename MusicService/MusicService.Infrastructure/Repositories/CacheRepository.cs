using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MusicService.Application.Options;
using MusicService.Domain.Interfaces;
using System.Text.Json;

namespace MusicService.Infrastructure.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDistributedCache _cache;
        private readonly RedisConfig _cacheOptions;

        public CacheRepository(IDistributedCache cache, IOptions<RedisConfig> options)
        {
            _cache = cache;
            _cacheOptions = options.Value;
        }

        public async Task SetAsync<TValue>(string key, TValue value, CancellationToken cancellationToken = default)
            where TValue : class
        {
            string serializedValue = JsonSerializer.Serialize(value);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheOptions.LifetimeMinutes)
            };

            await _cache.SetStringAsync(key, serializedValue, options, cancellationToken);
        }

        public async Task<TValue?> GetAsync<TValue>(string key, CancellationToken cancellationToken = default)
            where TValue : class
        {
            var serializedValue = await _cache.GetStringAsync(key, cancellationToken);

            if (serializedValue != null)
            {
                var value = JsonSerializer.Deserialize<TValue>(serializedValue);

                return value;
            }

            return null;
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _cache.RemoveAsync(key, cancellationToken);
        }
    }
}
