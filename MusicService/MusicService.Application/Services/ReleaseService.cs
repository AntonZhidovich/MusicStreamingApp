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
using System.Security.Claims;

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

        public async Task<PageResponse<ReleaseDto>> GetAllAsync(GetPageRequest request, CancellationToken cancellationToken = default)
        {
            var releases = await _unitOfWork.Releases.GetAllAsync(request.CurrentPage, request.PageSize);
            var allReleasesCount = await _unitOfWork.Releases.CountAsync(cancellationToken);

            return releases.GetPageResponse<Release, ReleaseDto>(allReleasesCount, request, _mapper);
        }

        public async Task<PageResponse<ReleaseDto>> GetAllFromAuthorAsync(GetPageRequest request, 
            string artistName, 
            CancellationToken cancellationToken = default)
        {
            var specification = new ReleasesFromAuthorSpecification(artistName);
            var authors = await _unitOfWork.Releases.ApplySpecificationAsync(specification, request.CurrentPage, request.PageSize, cancellationToken);
            var allSongsCount = await _unitOfWork.Releases.CountAsync(specification, cancellationToken);

            return authors.GetPageResponse<Release, ReleaseDto>(allSongsCount, request, _mapper);
        }

        public async Task AddSongToReleaseAsync(string releaseId, 
            AddSongToReleaseRequest request, 
            ClaimsPrincipal user, 
            CancellationToken cancellationToken = default)
        {
            var release = await GetDomainReleaseAsync(releaseId, cancellationToken);

            if (!user.IsInRole(UserRoles.admin)) { CheckIfUserIsMember(release.Authors, user); }

            await AddSongToGivenReleaseAsync(release, request, cancellationToken);
            SetReleaseType(release);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        public async Task CreateAsync(CreateReleaseRequest request, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            var release = _mapper.Map<Release>(request);
            release.Id = Guid.NewGuid().ToString();

            var authors = await _unitOfWork.Authors.GetByNameAsync(request.AuthorNames, cancellationToken);
            release.Authors.AddRange(authors);

            if (release.Authors.Count != authors.Count()) { throw new NotFoundException("Some authors could not be found."); }

            if (!user.IsInRole(UserRoles.admin)) { CheckIfUserIsMember(authors!, user); }

            foreach (var songRequest in request.Songs)
            {
                await AddSongToGivenReleaseAsync(release, songRequest, cancellationToken);
            }

            SetReleaseType(release);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        public async Task DeleteAsync(string id, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            var release = await GetDomainReleaseAsync(id, cancellationToken);
            var authors = await _unitOfWork.Authors.GetByNameAsync(release.Authors.Select(author => author.Name), cancellationToken);

            if (!user.IsInRole(UserRoles.admin)) { CheckIfUserIsMember(authors!, user); }

            _unitOfWork.Releases.Delete(release);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        public async Task RemoveSongFromReleaseAsync(string releaseId, string songId, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            var release = await GetDomainReleaseAsync(releaseId, cancellationToken);

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
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        public async Task UpdateAsync(string id, UpdateReleaseRequest request, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            var release = await GetDomainReleaseAsync(id, cancellationToken);

            if (!user.IsInRole(UserRoles.admin)) { CheckIfUserIsMember(release.Authors, user); }

            _mapper.Map(request, release);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        private async Task AddSongToGivenReleaseAsync(Release release, AddSongToReleaseRequest request, CancellationToken cancellationToken = default)
        {
            var song = _mapper.Map<Song>(request);
            await _songService.CheckIfSourceExistsAsync(song.SourceName, cancellationToken);

            foreach (var genreName in request.Genres)
            {
                var genre = await _unitOfWork.Genres.GetOrCreateAsync(genreName, cancellationToken);
                song.Genres.Add(genre);
            }

            song.Id = Guid.NewGuid().ToString();
            song.Release = release;
            release.SongsCount++;
            release.DurationMinutes += song.DurationMinutes;
            await _unitOfWork.Songs.CreateAsync(song, cancellationToken);
        }

        private async Task<Release> GetDomainReleaseAsync(string id, CancellationToken cancellationToken = default)
        {
            var release = await _unitOfWork.Releases.GetByIdAsync(id, cancellationToken);

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

            throw new AuthorizationException("Only author members can do this action.");
        }
    }
}
