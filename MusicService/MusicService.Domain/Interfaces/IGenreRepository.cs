using MusicService.Domain.Entities;

namespace MusicService.Domain.Interfaces
{
    public interface IGenreRepository
    {
        Task<IEnumerable<Genre>> GetAllAsync(int currentPage, int pageSize);
        Task<Genre?> GetByNameAsync(string name);
        Task CreateAsync(Genre genre);
        void Update(Genre genre);
        void Delete(Genre genre);
        Task SaveChangesAsync();
        Task<int> CountAsync();
    }
}
