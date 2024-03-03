using MusicService.Domain.Entities;

namespace MusicService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    }
}
