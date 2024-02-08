using MusicService.Application.Models;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.ReleaseService;
using System.Security.Claims;

namespace MusicService.Application.Interfaces
{
    public interface IReleaseService
    {
        Task<PageResponse<ReleaseDto>> GetAllAsync(GetPageRequest request);
        Task CreateAsync(CreateReleaseRequest request, ClaimsPrincipal user);
        Task DeleteAsync(string id, ClaimsPrincipal user);
        Task UpdateAsync(string id, UpdateReleaseRequest request, ClaimsPrincipal user);
        Task RemoveSongFromReleaseAsync(string releaseId, string songId, ClaimsPrincipal user);
        Task AddSongToReleaseAsync(string releaseId, AddSongToReleaseRequest request, ClaimsPrincipal user);
    }
}
