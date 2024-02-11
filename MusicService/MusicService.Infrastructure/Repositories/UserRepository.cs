﻿using Microsoft.EntityFrameworkCore;
using MusicService.Domain.Entities;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Data;

namespace MusicService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MusicDbContext _dbContext;

        public UserRepository(MusicDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Author?> GetAuthorByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            var user = await GetByUserNameAsync(userName, cancellationToken);

            return user?.Author;
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .Include(user => user.Author)
                .Where(user => user.Email.Trim().ToLower() == email.Trim().ToLower())
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
