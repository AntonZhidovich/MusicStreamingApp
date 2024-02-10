using MusicService.Domain.Entities;

namespace MusicService.Domain.Interfaces
{
    public interface IAuthorRepository : IBaseRepository<Author>
    {
        Task<IEnumerable<Author>> GetAllAsync(int currentPage, int pageSize);
        Task<Author?> GetByNameAsync(string name);
        Task<IEnumerable<Author?>> GetByNameAsync(IEnumerable<string> authorNames);
        bool UserIsMember(Author author, string userName);
    }
}
