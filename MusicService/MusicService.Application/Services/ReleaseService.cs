using AutoMapper;
using MusicService.Application.Interfaces;
using MusicService.Application.Models;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.ReleaseService;
using MusicService.Application.Specifications;
using MusicService.Domain.Constants;
using MusicService.Domain.Entities;
using MusicService.Domain.Exceptions;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Extensions;
using MusicService.Infrastructure.Specifications;
using System.Security.Claims;
using System.Xml.Linq;

namespace MusicService.Application.Services
{
    public class ReleaseService : IReleaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISongService _songService;
        private readonly IMapper _mapper;

        public ReleaseService(
            IUnitOfWork unitOfWork,
            ISongService songService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _songService = songService;
            _mapper = mapper;
        }

        public async Task<PageResponse<ReleaseDto>> GetAllAsync(GetPageRequest request)
        {
            var releases = await _unitOfWork.Releases.GetAllAsync(request.CurrentPage, request.PageSize);
            var allReleasesCount = await _unitOfWork.Releases.CountAsync();

            return releases.GetPageResponse<Release, ReleaseDto>(allReleasesCount, request, _mapper);
        }

        public async Task<PageResponse<ReleaseDto>> GetAllFromAuthorAsync(GetPageRequest request, string artistName)
        {
            var specification = new ReleasesFromAuthorSpecification(artistName);
            var authors = await _unitOfWork.Releases.ApplySpecificationAsync(specification, request.CurrentPage, request.PageSize);
            var allSongsCount = await _unitOfWork.Releases.CountAsync(specification);

            return authors.GetPageResponse<Release, ReleaseDto>(allSongsCount, request, _mapper);
        }

        public async Task AddSongToReleaseAsync(string releaseId, AddSongToReleaseRequest request, ClaimsPrincipal user)
        {
            var release = await GetDomainReleaseAsync(releaseId);

            if (!user.IsInRole(UserRoles.admin)) { CheckIfUserIsMember(release.Authors, user); }

            await AddSongToGivenReleaseAsync(release, request);
            SetReleaseType(release);
            await _unitOfWork.CommitAsync();
        }

        public async Task CreateAsync(CreateReleaseRequest request, ClaimsPrincipal user)
        {
            var release = _mapper.Map<Release>(request);
            release.Id = Guid.NewGuid().ToString();

            var authors = await _unitOfWork.Authors.GetByNameAsync(request.AuthorNames);

            foreach (var author in authors)
            { 
                if (author == null)
                {
                    throw new NotFoundException($"One of authors was not found.");
                }

                release.Authors.Add(author);
            }

            if (!user.IsInRole(UserRoles.admin)) { CheckIfUserIsMember(authors!, user); }

            foreach (var songRequest in request.Songs)
            {
                await AddSongToGivenReleaseAsync(release, songRequest);
            }

            SetReleaseType(release);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(string id, ClaimsPrincipal user)
        {
            var release = await GetDomainReleaseAsync(id);
            var authors = await _unitOfWork.Authors.GetByNameAsync(release.Authors.Select(author => author.Name));

            if (!user.IsInRole(UserRoles.admin)) { CheckIfUserIsMember(authors!, user); }

            _unitOfWork.Releases.Delete(release);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveSongFromReleaseAsync(string releaseId, string songId, ClaimsPrincipal user)
        {
            var release = await GetDomainReleaseAsync(releaseId);

            if (!user.IsInRole(UserRoles.admin)) { CheckIfUserIsMember(release.Authors, user); }

            var song = release.Songs.Find(song => song.Id == songId);

            if (song == null)
            {
                throw new NotFoundException("No song was found.");
            }

            release.Songs.Remove(song);
            release.DurationMinutes -= song.DurationMinutes;
            release.SongsCount--;
            _unitOfWork.Songs.Delete(song);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(string id, UpdateReleaseRequest request, ClaimsPrincipal user)
        {
            var release = await GetDomainReleaseAsync(id);

            if (!user.IsInRole(UserRoles.admin)) { CheckIfUserIsMember(release.Authors, user); }

            _mapper.Map(request, release);
            await _unitOfWork.CommitAsync();
        }

        private async Task AddSongToGivenReleaseAsync(Release release, AddSongToReleaseRequest request)
        {
            var song = _mapper.Map<Song>(request);
            await _songService.CheckIfSourceExistsAsync(song.SourceName);

            foreach (var genreName in request.Genres)
            {
                var genre = await _unitOfWork.Genres.GetOrCreateAsync(genreName);
                song.Genres.Add(genre);
            }

            song.Id = Guid.NewGuid().ToString();
            song.Release = release;
            release.SongsCount++;
            release.DurationMinutes += request.DurationMinutes;
            await _unitOfWork.Songs.CreateAsync(song);
        }

        private async Task<Release> GetDomainReleaseAsync(string id)
        {
            var release = await _unitOfWork.Releases.GetByIdAsync(id);

            if (release == null)
            {
                throw new NotFoundException("No release was found.");
            }

            return release;
        }

        private void SetReleaseType(Release release)
        {
            switch (release.SongsCount)
            {
                case 1:
                    release.Type = ReleaseTypes.Single;
                    break;
                case < ReleaseTypes.EpSongsCount:
                    release.Type = ReleaseTypes.EP;
                    break;
                default:
                    release.Type = ReleaseTypes.LpAlbum;
                    break;
            }
        }

        private void CheckIfUserIsMember(IEnumerable<Author> authors, ClaimsPrincipal user)
        {
            var currentUserName = user.Identity!.Name!;

            foreach (var author in authors)
            {
                if (_unitOfWork.Authors.UserIsMember(author, currentUserName)) { return; }
            }

            throw new UnauthorizedException("Only author members can do this action.");
        }
    }
}
