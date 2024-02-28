using MusicService.Domain.Interfaces;
using SubscriptionService.DataAccess.Entities;

namespace SubscriptionService.DataAccess.Repositories.Interfaces
{
    public interface ISubscriptionRepository : IBaseRepository<Subscription>
    {
        Task<IEnumerable<Subscription>> GetAllAsync(int currentPage, int pageSize, CancellationToken cancellationToken = default);
        Task<Subscription?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<Subscription?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}
