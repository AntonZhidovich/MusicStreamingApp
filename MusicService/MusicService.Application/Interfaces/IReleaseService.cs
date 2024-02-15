using MusicService.Application.Models;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.ReleaseService;
using System.Security.Claims;

namespace MusicService.Application.Interfaces
{
    public interface IReleaseService
    {
        Task<PageResponse<ReleaseShortDto>> GetAllAsync(GetPageRequest request, CancellationToken cancellationToken = default);
        Task<ReleaseDto> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<PageResponse<ReleaseShortDto>> GetAllFromAuthorAsync(GetPageRequest request, string artistName, CancellationToken cancellationToken = default);
        Task<ReleaseDto> CreateAsync(CreateReleaseRequest request, ClaimsPrincipal user, CancellationToken cancellationToken = default);
        Task DeleteAsync(string id, ClaimsPrincipal user, CancellationToken cancellationToken = default);
        Task<ReleaseShortDto> UpdateAsync(string id, UpdateReleaseRequest request, ClaimsPrincipal user, CancellationToken cancellationToken = default);
        Task RemoveSongFromReleaseAsync(string releaseId, string songId, ClaimsPrincipal user, CancellationToken cancellationToken = default);
        Task<SongDto> AddSongToReleaseAsync(string releaseId, AddSongToReleaseRequest request, ClaimsPrincipal user, CancellationToken cancellationToken = default);
    }
}
