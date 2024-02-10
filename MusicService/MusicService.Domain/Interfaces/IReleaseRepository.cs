using MusicService.Domain.Entities;

namespace MusicService.Domain.Interfaces
{
    public interface IReleaseRepository : IBaseRepository<Release>
    {
        Task<IEnumerable<Release>> GetAllAsync(int currentPage, int pageSize);
        Task<Release?> GetByIdAsync(string id);
    }
}
