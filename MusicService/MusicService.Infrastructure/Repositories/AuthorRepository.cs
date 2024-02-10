﻿using Microsoft.EntityFrameworkCore;
using MusicService.Domain.Entities;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Data;
using MusicService.Infrastructure.Extensions;

namespace MusicService.Infrastructure.Repositories
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(MusicDbContext dbContext) : base(dbContext) { }

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
                .Include(author => author.Users)
                .Where(author => author.Name.Trim().ToLower() == name.Trim().ToLower())
                .FirstOrDefaultAsync();

            return author;
        }

        public async Task<IEnumerable<Author?>> GetByNameAsync(IEnumerable<string> authorNames)
        {
            var authors = await _dbContext.Authors
                .Include(author => author.Users)
                .Where(author => authorNames.Contains(author.Name))
                .ToListAsync();

            return authors;
        }

        public bool UserIsMember(Author author, string userName)
        {
            var artistUserNames = author.Users.Select(user => user.UserName);

            return artistUserNames.Contains(userName);
        }
    }
}
