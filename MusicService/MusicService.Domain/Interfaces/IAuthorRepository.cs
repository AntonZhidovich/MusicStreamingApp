using MusicService.Domain.Entities;

namespace MusicService.Domain.Interfaces
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author?> GetByNameAsync(string name);
        Task CreateAsync(Author author);
        Task UpdateAsync(Author author);
        Task DeleteAsync(Author author);
        Task SaveChangesAsync();
    }
}
