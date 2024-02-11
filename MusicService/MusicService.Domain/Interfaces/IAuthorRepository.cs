using MusicService.Domain.Entities;

namespace MusicService.Domain.Interfaces
{
    public interface IAuthorRepository : IBaseRepository<Author>
    {
        Task<IEnumerable<Author>> GetAllAsync(int currentPage, int pageSize, CancellationToken cancellationToken = default);
        Task<Author?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<Author?>> GetByNameAsync(IEnumerable<string> authorNames, CancellationToken cancellationToken = default);
        bool UserIsMember(Author author, string userName);
    }
}
