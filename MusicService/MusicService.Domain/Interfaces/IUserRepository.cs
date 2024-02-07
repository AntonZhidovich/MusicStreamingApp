using MusicService.Domain.Entities;

namespace MusicService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUserNameAsync(string userName);
        Task<User?> GetByEmailAsync(string email);
        Task<Author?> GetAuthorByUserNameAsync(string userName);
    }
}
