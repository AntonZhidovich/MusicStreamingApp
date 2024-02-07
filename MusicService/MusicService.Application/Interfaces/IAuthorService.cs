using MusicService.Application.Models;
using MusicService.Application.Models.AuthorService;
using MusicService.Application.Models.DTOs;

namespace MusicService.Application.Interfaces
{
    public interface IAuthorService
    {
        Task<PageResponse<AuthorDto>> GetAllAsync(GetPageRequest request);
        Task<AuthorDto> GetByNameAsync(string name);
        Task CreateAsync(CreateAuthorRequest request);
        Task AddArtistToAuthorAsync(AuthorArtistRequest request);
        Task RemoveArtistFromAuthorAsync(AuthorArtistRequest request);
        Task DeleteAsync(string name);
        Task UpdateDesctiptionAsync(string name, UpdateAuthorDescriptionRequest request);
        Task BreakAuthorAsync(string name, BreakAuthorRequest request);
        Task UnbreakAuthorAsync(string name);
    }
}
