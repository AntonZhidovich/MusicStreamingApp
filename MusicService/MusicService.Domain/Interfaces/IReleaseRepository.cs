using MusicService.Domain.Entities;

namespace MusicService.Domain.Interfaces
{
    public interface IReleaseRepository : IBaseRepository<Release>
    {
        Task<IEnumerable<Release>> GetAllAsync(int currentPage, int pageSize, CancellationToken cancellationToken = default);
        Task<Release?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    }
}
