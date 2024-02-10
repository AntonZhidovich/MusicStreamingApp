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

        public async Task<IEnumerable<T>> ApplySpecificationAsync(ISpecification<T> specification, int currentPage, int pageSize)
        {
            return await _dbContext.Set<T>()
                .ApplySpecification(specification)
                .GetPage(currentPage, pageSize)
                .ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Set<T>().CountAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> specification)
        {
            return await _dbContext.Set<T>()
                .ApplySpecification(specification)
                .CountAsync();
        }

        public async Task CreateAsync(T entity)
        {
            await _dbContext.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _dbContext.Update(entity);
        }
    }
}
