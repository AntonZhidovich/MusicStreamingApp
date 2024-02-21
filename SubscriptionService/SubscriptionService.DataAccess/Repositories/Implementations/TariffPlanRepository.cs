using Microsoft.EntityFrameworkCore;
using SubscriptionService.DataAccess.Data;
using SubscriptionService.DataAccess.Entities;
using SubscriptionService.DataAccess.Extensions;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.DataAccess.Repositories.Implementations
{
    public class TariffPlanRepository : BaseRepository<TariffPlan>, ITariffPlanRepository
    {
        public TariffPlanRepository(SubscriptionDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<TariffPlan>> GetAllAsync(int currentPage, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _dbContext.TariffPlans
                .AsNoTracking()
                .OrderByDescending(plan => plan.AnnualFee)
                .GetPage(currentPage, pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<TariffPlan?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.TariffPlans
                .FirstOrDefaultAsync(plan => plan.Id == id, cancellationToken);
        }

        public async Task<TariffPlan?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _dbContext.TariffPlans
                .AsNoTracking()
                .Where(p => p.Name.Trim().ToLower() == name.Trim().ToLower())
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
