namespace SubscriptionService.DataAccess.Repositories.Interfaces
{
    public interface ICacheRepository
    {
        Task SetAsync<TValue>(string key, TValue value, CancellationToken cancellationToken = default)
            where TValue : class;

        Task<TValue?> GetAsync<TValue>(string key, CancellationToken cancellationToken = default)
            where TValue : class;

        Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    }
}
