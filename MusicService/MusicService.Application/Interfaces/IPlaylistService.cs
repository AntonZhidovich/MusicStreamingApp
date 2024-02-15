using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.PlaylistService;
using System.Security.Claims;

namespace MusicService.Application.Interfaces
{
    public interface IPlaylistService
    {
        Task CreateAsync(ClaimsPrincipal user, CreatePlaylistRequest request, CancellationToken cancellationToken = default);
        Task UpdateAsync(ClaimsPrincipal user, string id, UpdatePlaylistRequest request, CancellationToken cancellationToken = default);
        Task DeleteAsync(ClaimsPrincipal user, string id, CancellationToken cancellationToken = default);
        Task AddSongAsync(ClaimsPrincipal user, string playlistId, AddSongToPlaylistRequest request, CancellationToken cancellationToken = default);
        Task RemoveSongAsync(ClaimsPrincipal user, string playlistId, string songId, CancellationToken cancellationToken = default);
        Task<IEnumerable<PlaylistShortDto>> GetUserPlaylistsAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default);
        Task<PlaylistFullDto> GetFullPlaylistAsync(ClaimsPrincipal user, string id, CancellationToken cancellationToken = default);
    }
}
