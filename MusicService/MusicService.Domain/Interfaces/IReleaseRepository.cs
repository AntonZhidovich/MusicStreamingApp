using MusicService.Domain.Entities;

namespace MusicService.Domain.Interfaces
{
    public interface IReleaseRepository
    {
        Task<IEnumerable<Release>> GetAllAsync(int currentPage, int pageSize);
        Task<int> CountAsync();
        Task<Release?> GetByIdAsync(string id);
        Task CreateAsync(Release release);
        void Update(Release song);
        void Delete(Release song);
        Task SaveChangesAsync();
    }
}
