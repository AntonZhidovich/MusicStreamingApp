﻿using Microsoft.EntityFrameworkCore;
using MusicService.Domain.Entities;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Data;
using MusicService.Infrastructure.Extensions;

namespace MusicService.Infrastructure.Repositories
{
    public class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        public GenreRepository(MusicDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Genre>> GetAllAsync(int currentPage, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Genres
                .Include(genre => genre.Songs)
                .OrderBy(genre => genre.Name)
                .AsNoTracking()
                .GetPage(currentPage, pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<Genre?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var genre = await _dbContext.Genres
                .Include (genre => genre.Songs)
                .Where(genre => genre.Name.Trim().ToLower() == name.Trim().ToLower())
                .FirstOrDefaultAsync(cancellationToken);

            return genre;
        }

        public async Task<Genre> GetOrCreateAsync(string name, CancellationToken cancellationToken = default)
        {
            var genre = GetTrackedByName(name);

            if (genre == null)
            {
                genre = await GetByNameAsync(name, cancellationToken);

                if (genre == null)
                {
                    genre = new Genre { Id = Guid.NewGuid().ToString(), Name = name, Description = string.Empty };
                    
                    await CreateAsync(genre, cancellationToken);
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
    }
}
