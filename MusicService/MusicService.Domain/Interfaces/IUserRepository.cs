﻿using MusicService.Domain.Entities;

namespace MusicService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<Author?> GetAuthorByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    }
}
