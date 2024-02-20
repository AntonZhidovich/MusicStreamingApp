namespace SubscriptionService.DataAccess.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        public Task CommitChangesAsync(CancellationToken cancellationToken = default);
    }
}
