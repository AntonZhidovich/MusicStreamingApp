using Microsoft.EntityFrameworkCore;
using MusicService.Domain.Interfaces;
using SubscriptionService.DataAccess.Data;

namespace SubscriptionService.DataAccess.Repositories.Implementations
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly SubscriptionDbContext _dbContext;

        public BaseRepository(SubscriptionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().CountAsync(cancellationToken);
        }

        public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(entity, cancellationToken);
        }

        public void Delete(T entity)
        {
            _dbContext.Remove(entity);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _dbContext.Update(entity);
        }
    }
}
