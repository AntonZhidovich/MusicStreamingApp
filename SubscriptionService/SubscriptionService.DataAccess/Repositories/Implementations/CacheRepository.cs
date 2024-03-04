using Microsoft.Extensions.Caching.Distributed;
using SubscriptionService.DataAccess.Repositories.Interfaces;
using System.Text;
using System.Text.Json;

namespace SubscriptionService.DataAccess.Repositories.Implementations
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDistributedCache _cache;

        public CacheRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<TValue?> GetAsync<TValue>(string key, CancellationToken cancellationToken = default) 
            where TValue : class
        {
            var serializedValue = await _cache.GetStringAsync(key, cancellationToken);

            if (serializedValue == null)
            {
                return null;
            }

            var value = JsonSerializer.Deserialize<TValue>(serializedValue);

            return value;

        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _cache.RemoveAsync(key, cancellationToken);
        }

        public async Task SetAsync<TValue>(string key, TValue value, CancellationToken cancellationToken = default) 
            where TValue : class
        {
            var serializedValue = JsonSerializer.Serialize(value);

            await _cache.SetStringAsync(key, serializedValue, cancellationToken);
        }
    }
}
