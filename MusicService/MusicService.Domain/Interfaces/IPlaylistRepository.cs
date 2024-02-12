using MusicService.Domain.Entities;

namespace MusicService.Domain.Interfaces
{
    public interface IPlaylistRepository
    {
        Task CreateAsync(Playlist playlist, CancellationToken cancellationToken = default);
        Task UpdateAsync(Playlist playlist, CancellationToken cancellationToken = default);
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<Playlist?> GetAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Playlist>> GetUserPlaylistsAsync(string userName, CancellationToken cancellationToken = default);
    }
}
