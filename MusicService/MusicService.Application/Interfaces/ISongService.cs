using MusicService.Application.Models;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.SongService;

namespace MusicService.Application.Interfaces
{
    public interface ISongService
    {
        Task<PageResponse<SongDto>> GetAllAsync(GetPageRequest request);
        Task<SongDto> GetByIdAsync(string id);
        Task<PageResponse<SongDto>> GetSongsFromGenreAsync(GetPageRequest request, string genreName);
        Task<PageResponse<SongDto>> GetSongsByNameAsync(GetPageRequest request, string name);
        Task UpdateAsync(string id, UpdateSongRequest request);
        Task ChangeGenreDescriptionAsync(string name, ChangeGenreDescriptionRequest request);
        Task<PageResponse<GenreDto>> GetAllGenresAsync(GetPageRequest request);
        Task<GenreDto> GetGenreByNameAsync(string name);
        Task CheckIfSourceExistsAsync(string source);
        Task DeleteGenreAsync(string name);
    }
}
