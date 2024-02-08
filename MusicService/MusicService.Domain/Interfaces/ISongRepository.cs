using MusicService.Domain.Entities;

namespace MusicService.Domain.Interfaces
{
    public interface ISongRepository
    {
        Task<IEnumerable<Song>> GetAllAsync(int currentPage, int pageSize);
        Task<int> CountAsync();
        Task<Song?> GetByIdAsync(string id);
        Task CreateAsync(Song song);
        void Update(Song song);
        void Delete(Song song);
        Task SaveChangesAsync();
    }
}
