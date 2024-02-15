using MusicService.Application.Models;
using MusicService.Application.Models.AuthorService;
using MusicService.Application.Models.DTOs;
using System.Security.Claims;

namespace MusicService.Application.Interfaces
{
    public interface IAuthorService
    {
        Task<PageResponse<AuthorDto>> GetAllAsync(GetPageRequest request, CancellationToken cancellationToken = default);
        Task<AuthorDto> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<AuthorDto> CreateAsync(CreateAuthorRequest request, CancellationToken cancellationToken = default);
        Task AddUserToAuthorAsync(AuthorUserRequest request, ClaimsPrincipal currentUser, CancellationToken cancellationToken = default);
        Task RemoveUserFromAuthorAsync(AuthorUserRequest request, ClaimsPrincipal currentUser, CancellationToken cancellationToken = default);
        Task DeleteAsync(string name, ClaimsPrincipal currentUser, CancellationToken cancellationToken = default);
        Task<AuthorDto> UpdateAsync(string name, UpdateAuthorRequest request, ClaimsPrincipal currentUser, CancellationToken cancellationToken = default);
    }
}
