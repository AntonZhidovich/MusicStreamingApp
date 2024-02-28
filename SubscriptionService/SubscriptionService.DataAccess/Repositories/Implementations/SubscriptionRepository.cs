using Microsoft.EntityFrameworkCore;
using SubscriptionService.DataAccess.Data;
using SubscriptionService.DataAccess.Entities;
using SubscriptionService.DataAccess.Extensions;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.DataAccess.Repositories.Implementations
{
    public class SubscriptionRepository : BaseRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(SubscriptionDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Subscription>> GetAllAsync(int currentPage, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Subscriptions
                .AsNoTracking()
                .Include(subscription => subscription.TariffPlan)
                .OrderByDescending(subscription => subscription.SubscribedAt)
                .GetPage(currentPage, pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<Subscription?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Subscriptions
                .Include(subscription => subscription.TariffPlan)
                .AsNoTracking()
                .FirstOrDefaultAsync(subscription => subscription.Id == id, cancellationToken);
        }

        public async Task<Subscription?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Subscriptions
                .Include(subscription => subscription.TariffPlan)
                .AsNoTracking()
                .FirstOrDefaultAsync(subscription => subscription.UserId == userId,
                cancellationToken);
        }
    }
}
