using MusicService.Application.Models;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.SongService;

namespace MusicService.Application.Interfaces
{
    public interface ISongService
    {
        Task<PageResponse<SongDto>> GetAllAsync(GetPageRequest request);
        Task<SongDto> GetByIdAsync(string id);
        Task ChangeSongSourceAsync(string id, ChangeSongSourceRequest request);
        Task UpdateAsync(string id, UpdateSongRequest request);
        Task ChangeGenreDescriptionAsync(string name, ChangeGenreDescriptionRequest request);
        Task<PageResponse<GenreDto>> GetAllGenresAsync(GetPageRequest request);
        Task<GenreDto> GetGenreByNameAsync(string name);
        Task CheckIfSourceExists(string source);
        Task DeleteEmptyGenres();
    }
}
