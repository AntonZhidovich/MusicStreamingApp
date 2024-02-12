using MusicService.Domain.Entities;

namespace MusicService.Domain.Interfaces
{
    public interface ISongRepository : IBaseRepository<Song>
    {
        Task<IEnumerable<Song>> GetAllAsync(int currentPage, int pageSize, CancellationToken cancellationToken = default);
        Task<Song?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Song>> GetByIdAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
    }
}
