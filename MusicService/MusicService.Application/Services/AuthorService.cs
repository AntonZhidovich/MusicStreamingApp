using AutoMapper;
using Microsoft.AspNetCore.Http;
using MusicService.Application.Interfaces;
using MusicService.Application.Models;
using MusicService.Application.Models.AuthorService;
using MusicService.Domain.Constants;
using MusicService.Domain.Entities;
using MusicService.Domain.Exceptions;
using MusicService.Domain.Interfaces;

namespace MusicService.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthorService(
            IAuthorRepository authorRepository, 
            IMapper mapper,
            IUserRepository userRepository,
            IHttpContextAccessor contextAccessor )
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task AddArtistToAuthorAsync(AuthorArtistRequest request)
        {
            var user = await GetDomainUserAsync(request.ArtistUserName);

            if (!user.Roles.Contains(UserRoles.creator))
            {
                throw new BadRequestException("User does not have a creator role.");
            }

            if (user.Author != null)
            {
                throw new BadRequestException("User is already in author group.");
            }

            var author = await GetDomainAuthorAsync(request.AuthorName);
            CheckIfMember(author);
            author.Users.Add(user);
            await _authorRepository.SaveChangesAsync();
        }

        public async Task BreakAuthorAsync(string name, BreakAuthorRequest request)
        {
            var author = await GetDomainAuthorAsync(name);
            author.IsBroken = true;
            author.BrokenAt = request.BrokenAt;
            await _authorRepository.SaveChangesAsync();
        }

        public async Task CreateAsync(CreateAuthorRequest request)
        {
            List<User> artists = new List<User>();

            foreach (var username in request.UserNames)
            {
                var user = await GetDomainUserAsync(username);

                if (!user.Roles.Contains(UserRoles.creator))
                {
                    throw new BadRequestException($"{username} does not have a {UserRoles.creator} role.");
                }

                if (user.Author != null)
                {
                    throw new BadRequestException($"{username} is already in {user.Author.Name}");
                }

                artists.Add(user);
            }

            var author = _mapper.Map<Author>(request);
            author.Id = Guid.NewGuid().ToString();
            author.Users = artists;
            await _authorRepository.CreateAsync(author);
        }

        public async Task DeleteAsync(string name)
        {
            var author = await GetDomainAuthorAsync(name);
            CheckIfMember(author);
            await _authorRepository.DeleteAsync(author);
        }

        public async Task<IEnumerable<AuthorDto>> GetAllAsync()
        {
            var authors = await _authorRepository.GetAllAsync();
            
            return _mapper.Map<IEnumerable<AuthorDto>>(authors);
        }

        public async Task<AuthorDto> GetByNameAsync(string name)
        {
            var author = await GetDomainAuthorAsync(name);

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task RemoveArtistFromAuthorAsync(AuthorArtistRequest request)
        {
            var author = await GetDomainAuthorAsync(request.AuthorName);
            CheckIfMember(author);
            var user = await GetDomainUserAsync(request.ArtistUserName);

            if (user == null)
            {
                throw new NotFoundException("No artist with such UserName was found.");
            }

            if (user.Author != author)
            {
                throw new NotFoundException("No user was found in the specified author group.");
            }

            author.Users.Remove(user);
            await _authorRepository.SaveChangesAsync();
        }

        public async Task UnbreakAuthorAsync(string name)
        {
            var author = await GetDomainAuthorAsync(name);
            author.IsBroken = false;
            await _authorRepository.SaveChangesAsync();
        }

        public async Task UpdateDesctiptionAsync(string name, UpdateAuthorDescriptionRequest request)
        {
            var author = await GetDomainAuthorAsync(name);
            author.Description = request.NewDescription;
            await _authorRepository.SaveChangesAsync();
        }

        private async Task<Author> GetDomainAuthorAsync(string name)
        {
            var author = await _authorRepository.GetByNameAsync(name);

            if(author == null)
            {
                throw new NotFoundException("No author was found.");
            }

            return author;
        }

        private async Task<User> GetDomainUserAsync(string userName)
        {
            var user = await _userRepository.GetByUserNameAsync(userName);

            if (user == null)
            {
                throw new NotFoundException("No user was found");
            }

            return user;
        }

        private void CheckIfMember(Author author)
        {
            var user = _contextAccessor.HttpContext.User;

            if (user.IsInRole(UserRoles.admin))
            {
                return;
            }

            var currentUserName = user.Identity!.Name;
            var artistUserNames = author.Users.Select(user => user.UserName);

            if (!artistUserNames.Contains(currentUserName)) 
            {
                throw new UnauthorizedException("Only group members can do this action.");
            }
        }
    }
}
