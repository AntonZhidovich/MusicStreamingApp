﻿using Microsoft.EntityFrameworkCore;
using MusicService.Domain.Entities;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Data;
using MusicService.Infrastructure.Extensions;

namespace MusicService.Infrastructure.Repositories
{
    public class SongRepository : BaseRepository<Song>, ISongRepository
    {
        public SongRepository(MusicDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Song>> GetAllAsync(int currentPage, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Songs
                .Include(song => song.Genres)
                .Include(song => song.Release)
                .ThenInclude(release => release.Authors)
                .OrderByDescending(song => song.Release.ReleasedAt)
                .GetPage(currentPage, pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<Song?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var song = await _dbContext.Songs
                .Include(song => song.Genres)
                .Include(song => song.Release)
                .ThenInclude(release => release.Authors)
                .FirstOrDefaultAsync(song => song.Id == id, cancellationToken);

            return song;
        }
    }
}
