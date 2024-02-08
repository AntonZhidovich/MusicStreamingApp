using Microsoft.EntityFrameworkCore;
using MusicService.Domain.Entities;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Data;
using MusicService.Infrastructure.Extensions;

namespace MusicService.Infrastructure.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly MusicDbContext _dbContext;

        public GenreRepository(MusicDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Genres.CountAsync();
        }

        public async Task CreateAsync(Genre genre)
        {
            await _dbContext.AddAsync(genre);
        }

        public void Delete(Genre genre)
        {
            _dbContext.Remove(genre);
        }

        public void DeleteAllEmpty()
        {
            _dbContext.RemoveRange(
                _dbContext.Genres
                .Include(genre => genre.Songs)
                .Where(genre => genre.Songs.Count == 0));
        }

        public async Task<IEnumerable<Genre>> GetAllAsync(int currentPage, int pageSize)
        {
            return await _dbContext.Genres
                .Include(genre => genre.Songs)
                .OrderBy(genre => genre.Name)
                .GetPage(currentPage, pageSize)
                .ToListAsync();
        }

        public async Task<Genre?> GetByNameAsync(string name)
        {
            var genre = await _dbContext.Genres
                .Include (genre => genre.Songs)
                .Where(genre => genre.Name.Trim().ToLower() == name.Trim().ToLower())
                .FirstOrDefaultAsync();

            return genre;
        }

        public async Task<Genre> GetOrCreateAsync(string name)
        {
            var genre = GetTrackedByName(name);

            if (genre == null)
            {
                genre = await GetByNameAsync(name);

                if (genre == null)
                {
                    genre = new Genre { Id = Guid.NewGuid().ToString(), Name = name, Description = string.Empty };
                    await CreateAsync(genre);
                }
            }

            return genre;
        }

        public Genre? GetTrackedByName(string name)
        {
            return _dbContext.ChangeTracker.Entries<Genre>()
                .Select(entry => entry.Entity)
                .FirstOrDefault(genre => genre.Name == name);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Update(Genre genre)
        {
            _dbContext.Update(genre);
        }
    }
}
