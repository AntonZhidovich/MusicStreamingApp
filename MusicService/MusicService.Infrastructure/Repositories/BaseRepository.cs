using Microsoft.EntityFrameworkCore;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Data;
using MusicService.Infrastructure.Extensions;

namespace MusicService.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly MusicDbContext _dbContext;

        public BaseRepository(MusicDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> ApplySpecificationAsync(
            ISpecification<T> specification, 
            int currentPage, 
            int pageSize, 
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>()
                .ApplySpecification(specification)
                .GetPage(currentPage, pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().CountAsync(cancellationToken);
        }

        public async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>()
                .ApplySpecification(specification)
                .CountAsync(cancellationToken);
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
