using MusicService.Application.Models;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.SongService;

namespace MusicService.Application.Interfaces
{
    public interface ISongService
    {
        Task<PageResponse<SongDto>> GetAllAsync(GetPageRequest request, CancellationToken cancellationToken = default);
        Task<SongDto> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task UpdateAsync(string id, UpdateSongRequest request, CancellationToken cancellationToken = default);
        Task ChangeGenreDescriptionAsync(string name, ChangeGenreDescriptionRequest request, CancellationToken cancellationToken = default);
        Task<PageResponse<GenreDto>> GetAllGenresAsync(GetPageRequest request, CancellationToken cancellationToken = default);
        Task<PageResponse<SongDto>> GetSongsFromGenreAsync(GetPageRequest request, string genreName, CancellationToken cancellationToken = default);
        Task<PageResponse<SongDto>> GetSongsByNameAsync(GetPageRequest request, string name, CancellationToken cancellationToken = default);
        Task<GenreDto> GetGenreByNameAsync(string name, CancellationToken cancellationToken = default);
        Task CheckIfSourceExistsAsync(string source, CancellationToken cancellationToken = default);
        Task DeleteGenreAsync(string name, CancellationToken cancellationToken = default);
    }
}
