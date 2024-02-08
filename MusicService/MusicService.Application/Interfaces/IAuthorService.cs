using MusicService.Application.Models;
using MusicService.Application.Models.AuthorService;
using MusicService.Application.Models.DTOs;
using System.Security.Claims;

namespace MusicService.Application.Interfaces
{
    public interface IAuthorService
    {
        Task<PageResponse<AuthorDto>> GetAllAsync(GetPageRequest request);
        Task<AuthorDto> GetByNameAsync(string name);
        Task CreateAsync(CreateAuthorRequest request);
        Task AddArtistToAuthorAsync(AuthorArtistRequest request, ClaimsPrincipal currentUser);
        Task RemoveArtistFromAuthorAsync(AuthorArtistRequest request, ClaimsPrincipal currentUser);
        Task DeleteAsync(string name, ClaimsPrincipal currentUser);
        Task UpdateDesctiptionAsync(string name, UpdateAuthorDescriptionRequest request, ClaimsPrincipal currentUser);
        Task BreakAuthorAsync(string name, BreakAuthorRequest request);
        Task UnbreakAuthorAsync(string name);
    }
}
