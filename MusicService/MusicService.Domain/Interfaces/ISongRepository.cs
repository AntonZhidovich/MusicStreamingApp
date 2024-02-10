using MusicService.Domain.Entities;

namespace MusicService.Domain.Interfaces
{
    public interface ISongRepository : IBaseRepository<Song>
    {
        Task<IEnumerable<Song>> GetAllAsync(int currentPage, int pageSize);
        Task<Song?> GetByIdAsync(string id);
    }
}
