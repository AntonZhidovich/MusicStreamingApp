using MusicService.Domain.Entities;

namespace MusicService.Domain.Interfaces
{
    public interface IGenreRepository : IBaseRepository<Genre>
    {
        Task<IEnumerable<Genre>> GetAllAsync(int currentPage, int pageSize);
        Task<Genre?> GetByNameAsync(string name);
        Task<Genre> GetOrCreateAsync(string name);
        Genre? GetTrackedByName(string name);
    }
}
