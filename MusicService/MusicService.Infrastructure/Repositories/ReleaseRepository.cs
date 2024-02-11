using Microsoft.EntityFrameworkCore;
using MusicService.Domain.Entities;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Data;
using MusicService.Infrastructure.Extensions;

namespace MusicService.Infrastructure.Repositories
{
    public class ReleaseRepository : BaseRepository<Release>, IReleaseRepository
    {
        public ReleaseRepository(MusicDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Release>> GetAllAsync(int currentPage, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Releases
                .Include(release => release.Authors)
                .Include(release => release.Songs)
                .OrderBy(release => release.Name)
                .GetPage(currentPage, pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<Release?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Releases
                .Include(release => release.Authors)
                .Include(release => release.Songs)
                .FirstOrDefaultAsync(release => release.Id == id, cancellationToken);
        }
    }
}
