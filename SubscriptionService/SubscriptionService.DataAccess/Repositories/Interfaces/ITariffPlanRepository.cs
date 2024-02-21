using MusicService.Domain.Interfaces;
using SubscriptionService.DataAccess.Entities;

namespace SubscriptionService.DataAccess.Repositories.Interfaces
{
    public interface ITariffPlanRepository : IBaseRepository<TariffPlan>
    {
        Task<IEnumerable<TariffPlan>> GetAllAsync(int currentPage, int pageSize, CancellationToken cancellationToken = default);
        Task<TariffPlan?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<TariffPlan?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
