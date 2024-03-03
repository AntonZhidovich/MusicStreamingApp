using MusicService.Application.Models;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.SongService;

namespace MusicService.Application.Interfaces
{
    public interface IGenreService
    {
        Task<PageResponse<GenreDto>> GetAllAsync(GetPageRequest request, CancellationToken cancellationToken = default);
        Task<GenreDto> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<GenreDto> UpdateAsync(string name, UpdateGenreRequest request, CancellationToken cancellationToken = default);
        Task DeleteAsync(string name, CancellationToken cancellationToken = default);
    }
}
