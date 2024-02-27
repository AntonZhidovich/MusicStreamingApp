using Microsoft.EntityFrameworkCore;
using MusicService.Domain.Entities;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Data;

namespace MusicService.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(MusicDbContext dbContext) : base(dbContext) { }

        public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .Include(user => user.Author)
                .Where(user => user.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .Include(user => user.Author)
                .Where(user => user.UserName.Trim().ToLower() == userName.Trim().ToLower())
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
