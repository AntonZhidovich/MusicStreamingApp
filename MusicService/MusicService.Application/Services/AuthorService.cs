using AutoMapper;
using MusicService.Application.Interfaces;
using MusicService.Application.Models;
using MusicService.Application.Models.AuthorService;
using MusicService.Application.Models.DTOs;
using MusicService.Domain.Constants;
using MusicService.Domain.Entities;
using MusicService.Domain.Exceptions;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Extensions;
using System.Security.Claims;

namespace MusicService.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserServiceGrpcClient _userServiceClient;

        public AuthorService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IUserServiceGrpcClient userServiceClient)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userServiceClient = userServiceClient;
        }

        public async Task AddUserToAuthorAsync(AuthorUserRequest request, ClaimsPrincipal currentUser, CancellationToken cancellationToken = default)
        {
            var user = await GetDomainUserAsync(request.UserName, cancellationToken);

            await CheckIfUserIsInRoleAsync(user, UserRoles.creator, cancellationToken);

            if (user.Author != null)
            {
                throw new BadRequestException(ExceptionMessages.UserAlreadyInAuthor);
            }

            var author = await GetDomainAuthorAsync(request.AuthorName, cancellationToken);

            if (!currentUser.IsInRole(UserRoles.admin)) { CheckIfUserIsMember(author, currentUser); }

            author.Users.Add(user);
            _unitOfWork.Authors.Update(author);
            
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<AuthorDto> CreateAsync(CreateAuthorRequest request, CancellationToken cancellationToken = default)
        {
            if (await _unitOfWork.Authors.GetByNameAsync(request.Name, cancellationToken) != null)
            {
                throw new BadRequestException(ExceptionMessages.AuthorAlreadyExists);
            }

            List<User> artists = new List<User>();

            foreach (var username in request.UserNames)
            {
                var user = await GetDomainUserAsync(username, cancellationToken);

                await CheckIfUserIsInRoleAsync(user, UserRoles.creator, cancellationToken);

                if (user.Author != null)
                {
                    throw new BadRequestException(ExceptionMessages.UserAlreadyInAuthor);
                }

                artists.Add(user);
            }

            var author = _mapper.Map<Author>(request);
            author.Id = Guid.NewGuid().ToString();
            author.Users = artists;
            
            await _unitOfWork.Authors.CreateAsync(author, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task DeleteAsync(string name, ClaimsPrincipal currentUser, CancellationToken cancellationToken = default)
        {
            var author = await GetDomainAuthorAsync(name, cancellationToken);

            if (!currentUser.IsInRole(UserRoles.admin)) { CheckIfUserIsMember(author, currentUser); }

            _unitOfWork.Authors.Delete(author);
           
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<PageResponse<AuthorDto>> GetAllAsync(GetPageRequest request, CancellationToken cancellationToken = default)
        {
            var authors = await _unitOfWork.Authors.GetAllAsync(request.CurrentPage, request.PageSize, cancellationToken);
            int allAuthorsCount = await _unitOfWork.Authors.CountAsync(cancellationToken);

            return authors.GetPageResponse<Author, AuthorDto>(allAuthorsCount, request, _mapper);
        }

        public async Task<AuthorDto> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var author = await GetDomainAuthorAsync(name, cancellationToken);

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task RemoveUserFromAuthorAsync(AuthorUserRequest request, 
            ClaimsPrincipal currentUser, 
            CancellationToken cancellationToken = default)
        {
            var author = await GetDomainAuthorAsync(request.AuthorName, cancellationToken);

            if (!currentUser.IsInRole(UserRoles.admin)) { CheckIfUserIsMember(author, currentUser); }

            var user = await GetDomainUserAsync(request.UserName, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(ExceptionMessages.UserNotFound);
            }

            if (user.Author != author)
            {
                throw new NotFoundException(ExceptionMessages.UserNotFound);
            }

            author.Users.Remove(user);
            
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<AuthorDto> UpdateAsync(string name, 
            UpdateAuthorRequest request, 
            ClaimsPrincipal currentUser, 
            CancellationToken cancellationToken = default)
        {
            var author = await GetDomainAuthorAsync(name, cancellationToken);

            if (!currentUser.IsInRole(UserRoles.admin)) { CheckIfUserIsMember(author, currentUser); }

            _mapper.Map(request, author);
            _unitOfWork.Authors.Update(author);
           
            await _unitOfWork.CommitAsync(cancellationToken);

            return _mapper.Map<AuthorDto>(author);
        }

        private async Task<Author> GetDomainAuthorAsync(string name, CancellationToken cancellationToken = default)
        {
            var author = await _unitOfWork.Authors.GetByNameAsync(name, cancellationToken);

            if (author == null)
            {
                throw new NotFoundException(ExceptionMessages.AuthorNotFound);
            }

            return author;
        }

        private async Task<User> GetDomainUserAsync(string userName, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.Users.GetByUserNameAsync(userName, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(ExceptionMessages.UserNotFound);
            }

            return user;
        }

        private void CheckIfUserIsMember(Author author, ClaimsPrincipal currentUser)
        {
            var currentUserName = currentUser.Identity!.Name!;

            if (!_unitOfWork.Authors.UserIsMember(author, currentUserName))
            {
                throw new AuthorizationException(ExceptionMessages.NotAuthorMember);
            }
        }

        private async Task CheckIfUserIsInRoleAsync(User user, string  roleName, CancellationToken cancellationToken = default)
        {
            var userIsInRole = await _userServiceClient.UserIsInRoleAsync(user.Id, roleName, cancellationToken);

            if (!userIsInRole)
            {
                throw new BadRequestException(ExceptionMessages.UserIsNotCreator);
            }
        }
    }
}
