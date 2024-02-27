﻿using MusicService.Domain.Entities;

namespace MusicService.Domain.Interfaces
{
    public interface IPlaylistRepository
    {
        Task CreateAsync(Playlist playlist, CancellationToken cancellationToken = default);
        Task UpdateAsync(Playlist playlist, CancellationToken cancellationToken = default);
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<Playlist?> GetAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Playlist>> GetUserPlaylistsAsync(string userName, int? maxCount = null, CancellationToken cancellationToken = default);
        Task<int> CountAsync(string userName, CancellationToken cancellationToken = default);
        Task<int?> GetUserMaxPlaylistCountAsync(string userName, CancellationToken cancellationToken = default);
        Task UpsertUserPlaylistTariffAsync(UserPlaylistTariff tariff, CancellationToken cancellationToken = default);
        Task DeleteUserPlaylistTariffAsync(string userName, CancellationToken cancellationToken = default);
        Task DeleteUserPlaylistsAsync(string userName, CancellationToken cancellationToken = default);
    }
}
