namespace SubscriptionService.DataAccess.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        public ISubscriptionRepository Subscriptions { get; }
        public ITariffPlanRepository TariffPlans { get; }

        public Task CommitAsync(CancellationToken cancellationToken = default);
    }
}
