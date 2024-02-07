using Microsoft.EntityFrameworkCore;
using MusicService.Domain.Entities;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Data;
using MusicService.Infrastructure.Extensions;

namespace MusicService.Infrastructure.Repositories
{
    public class SongRepository : ISongRepository
    {
        private readonly MusicDbContext _dbContext;

        public SongRepository(MusicDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Songs.CountAsync();
        }

        public async Task CreateAsync(Song song)
        {
            await _dbContext.AddAsync(song);
        }

        public void Delete(Song song)
        {
            _dbContext.Remove(song);
        }

        public async Task<IEnumerable<Song>> GetAllAsync(int currentPage, int pageSize)
        {
            return await _dbContext.Songs
                .Include(song => song.Genres)
                .Include(song => song.Release)
                    .ThenInclude(release => release.Authors)
                .OrderByDescending(song => song.Release.CreatedAt)
                .GetPage(currentPage, pageSize)
                .ToListAsync();
        }

        public async Task<Song?> GetByIdAsync(string id)
        {
            var song = await _dbContext.Songs
                .Include(song => song.Genres)
                .Include(song => song.Release)
                    .ThenInclude(release => release.Authors)
                .FirstOrDefaultAsync(song => song.Id == id);

            return song;
        }

        public async Task<IEnumerable<Song>> GetByTitleAsync(string title, int currentPage, int pageSize)
        {
            var songs = await _dbContext.Songs
                .Include(song => song.Genres)
                .Include(song => song.Release)
                    .ThenInclude(release => release.Authors)
                .Where(song => song.Title.Trim().ToLower() == title.Trim().ToLower())
                .OrderByDescending(song => song.Release.CreatedAt)
                .GetPage(currentPage, pageSize)
                .ToListAsync();

            return songs;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Update(Song song)
        {
            _dbContext.Update(song);
        }
    }
}
