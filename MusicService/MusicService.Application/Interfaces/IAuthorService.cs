using MusicService.Application.Models;
using MusicService.Application.Models.AuthorService;

namespace MusicService.Application.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDto>> GetAllAsync();
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
