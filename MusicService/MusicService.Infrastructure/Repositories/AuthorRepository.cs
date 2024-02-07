using Microsoft.EntityFrameworkCore;
using MusicService.Domain.Entities;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Data;

namespace MusicService.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly MusicDbContext _dbContext;

        public AuthorRepository(MusicDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Author author)
        {
            await _dbContext.AddAsync(author);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(Author author)
        {
            _dbContext.Remove(author);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            var authors = await _dbContext.Authors
                .Include(author => author.Users)
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

        public async Task UpdateAsync(Author author)
        {
            _dbContext.Update(author);
            await SaveChangesAsync();
        }
    }
}
