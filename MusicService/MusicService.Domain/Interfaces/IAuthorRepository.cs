using MusicService.Domain.Entities;

namespace MusicService.Domain.Interfaces
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAsync(int currentPage, int pageSize);
        Task<Author?> GetByNameAsync(string name);
        Task<int> CountAsync();
        Task CreateAsync(Author author);
        void Update(Author author);
        void Delete(Author author);
        Task SaveChangesAsync();
    }
}
