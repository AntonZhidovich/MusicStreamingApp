using MusicService.Domain.Entities;

namespace MusicService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUserNameAsync(string userName);
        Task<Author?> GetAuthorByUserNameAsync(string userName);
    }
}
