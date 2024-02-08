using Microsoft.EntityFrameworkCore;
using MusicService.Domain.Entities;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Data;
using MusicService.Infrastructure.Extensions;

namespace MusicService.Infrastructure.Repositories
{
    public class ReleaseRepository : IReleaseRepository
    {
        private readonly MusicDbContext _dbContext;

        public ReleaseRepository(MusicDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Releases.CountAsync();
        }

        public async Task CreateAsync(Release release)
        {
            await _dbContext.AddAsync(release);
        }

        public void Delete(Release song)
        {
            _dbContext.Releases.Remove(song);
        }

        public async Task<IEnumerable<Release>> GetAllAsync(int currentPage, int pageSize)
        {
            return await _dbContext.Releases
                .Include(release => release.Authors)
                .Include(release => release.Songs)
                .OrderBy(release => release.Name)
                .GetPage(currentPage, pageSize)
                .ToListAsync();
        }

        public async Task<Release?> GetByIdAsync(string id)
        {
            return await _dbContext.Releases
                .Include(release => release.Authors)
                .Include(release => release.Songs)
                .FirstOrDefaultAsync(release => release.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Update(Release song)
        {
            _dbContext.Update(song);
        }
    }
}
