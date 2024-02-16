using MusicService.Application.Models;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.SongService;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;

namespace MusicService.Application.Interfaces
{
    public interface ISongService
    {
        Task<PageResponse<SongDto>> GetAllAsync(GetPageRequest request, CancellationToken cancellationToken = default);
        Task<SongDto> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<SongDto> UpdateAsync(string id, UpdateSongRequest request, CancellationToken cancellationToken = default);
        Task<PageResponse<SongDto>> GetSongsByTitleAsync(GetPageRequest request, string title, CancellationToken cancellationToken = default);
        Task<PageResponse<SongDto>> GetSongsFromGenreAsync(GetPageRequest request, string genreName, CancellationToken cancellationToken = default);

        Task CheckIfSourceExistsAsync(string fullSourceName, CancellationToken cancellationToken = default);
        Task UploadSongSourceAsync(ClaimsPrincipal user, UploadSongSourceRequest request, CancellationToken cancellationToken = default);
        Task RemoveSongSourceAsync(ClaimsPrincipal user, string sourceName, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetSourcesAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default);

        Task<Stream> GetSourceStreamAsync(ClaimsPrincipal user, 
            string authorName,
            string sourceName,
            Stream outputStream,
            RangeItemHeaderValue? range = null,
            CancellationToken cancellationToken = default);
    }
}
