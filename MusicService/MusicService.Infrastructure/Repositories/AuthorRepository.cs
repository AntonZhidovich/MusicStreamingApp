using Microsoft.EntityFrameworkCore;
using MusicService.Domain.Entities;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Data;
using MusicService.Infrastructure.Extensions;

namespace MusicService.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly MusicDbContext _dbContext;

        public AuthorRepository(MusicDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Authors.CountAsync();
        }

        public async Task CreateAsync(Author author)
        {
            await _dbContext.AddAsync(author);
        }

        public void Delete(Author author)
        {
            _dbContext.Remove(author);
        }

        public async Task<IEnumerable<Author>> GetAllAsync(int currentPage, int pageSize)
        {
            var authors = await _dbContext.Authors
                .Include(author => author.Users)
                .OrderByDescending(author => author.Name)
                .GetPage(currentPage, pageSize)
                .ToListAsync();

            return authors;
        }

        public Task<Author?> GetByNameAsync(string name)
        {
            var author = _dbContext.Authors
                .Where(author => author.Name.Trim().ToLower() == name.Trim().ToLower())
                .Include(author => author.Users)
                .FirstOrDefaultAsync();

            return author;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Update(Author author)
        {
            _dbContext.Update(author);
        }

        public bool UserIsMember(Author author, string userName)
        {
            var artistUserNames = author.Users.Select(user => user.UserName);

            return artistUserNames.Contains(userName);
        }
    }
}
