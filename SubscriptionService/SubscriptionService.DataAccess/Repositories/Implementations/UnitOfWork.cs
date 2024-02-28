using Microsoft.Extensions.DependencyInjection;
using SubscriptionService.DataAccess.Data;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.DataAccess.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SubscriptionDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;
        private ISubscriptionRepository? _subscriptions = null;
        private ITariffPlanRepository? _tariffPlans = null;

        public ISubscriptionRepository Subscriptions => _subscriptions ??= _serviceProvider.GetService<ISubscriptionRepository>()!;
        public ITariffPlanRepository TariffPlans => _tariffPlans ??= _serviceProvider.GetService<ITariffPlanRepository>()!;

        public UnitOfWork(
            SubscriptionDbContext dbContext,
            IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
