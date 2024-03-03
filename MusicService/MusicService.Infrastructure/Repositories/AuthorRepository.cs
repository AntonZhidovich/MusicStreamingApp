using Microsoft.EntityFrameworkCore;
using MusicService.Domain.Entities;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Data;
using MusicService.Infrastructure.Extensions;

namespace MusicService.Infrastructure.Repositories
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(MusicDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Author>> GetAllAsync(int currentPage, int pageSize, CancellationToken cancellationToken = default)
        {
            var authors = await _dbContext.Authors
                .Include(author => author.Users)
                .OrderByDescending(author => author.Name)
                .AsNoTracking()
                .GetPage(currentPage, pageSize)
                .ToListAsync(cancellationToken);

            return authors;
        }

        public async Task<Author?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var author = await _dbContext.Authors
                .Include(author => author.Users)
                .Where(author => author.Name.Trim().ToLower() == name.Trim().ToLower())
                .FirstOrDefaultAsync(cancellationToken);

            return author;
        }

        public async Task<IEnumerable<Author>> GetByNameAsync(IEnumerable<string> authorNames, CancellationToken cancellationToken = default)
        {
            var authors = await _dbContext.Authors
                .Include(author => author.Users)
                .Where(author => authorNames.Contains(author.Name))
                .ToListAsync(cancellationToken);

            return authors;
        }

        public bool UserIsMember(Author author, string userName)
        {
            var artistUserNames = author.Users.Select(user => user.UserName.Trim().ToLower());

            return artistUserNames.Contains(userName.Trim().ToLower());
        }
    }
}
