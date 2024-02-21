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
                .OrderByDescending(subscription => subscription.SubscribedAt)
                .GetPage(currentPage, pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<Subscription?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Subscriptions
                .FirstOrDefaultAsync(subscription => subscription.Id == id, cancellationToken);
        }

        public async Task<Subscription?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Subscriptions
                .FirstOrDefaultAsync(subscription => subscription.UserName.Trim().ToLower() == userName.Trim().ToLower(),
                cancellationToken);
        }
    }
}
