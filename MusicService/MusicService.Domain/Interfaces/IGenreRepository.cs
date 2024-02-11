using MusicService.Domain.Entities;

namespace MusicService.Domain.Interfaces
{
    public interface IGenreRepository : IBaseRepository<Genre>
    {
        Task<IEnumerable<Genre>> GetAllAsync(int currentPage, int pageSize, CancellationToken cancellationToken = default);
        Task<Genre?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<Genre> GetOrCreateAsync(string name, CancellationToken cancellationToken = default);
        Genre? GetTrackedByName(string name);
    }
}
