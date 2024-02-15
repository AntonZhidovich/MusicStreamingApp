using AutoMapper;
using MusicService.Application.Interfaces;
using MusicService.Application.Models;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.SongService;
using MusicService.Domain.Constants;
using MusicService.Domain.Entities;
using MusicService.Domain.Exceptions;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Extensions;
using MusicService.Infrastructure.Specifications;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using MusicService.Application.Specifications;

namespace MusicService.Application.Services
{
    public class SongService : ISongService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISongSourceRepository _sourceRepository;

        public SongService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ISongSourceRepository sourceRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sourceRepository = sourceRepository;
        }

        public async Task CheckIfSourceExistsAsync(string fullSourceName, CancellationToken cancellationToken = default)
        {
            await _sourceRepository.SourceExistsAsync(fullSourceName);
        }

        public async Task<PageResponse<SongDto>> GetAllAsync(GetPageRequest request, CancellationToken cancellationToken = default)
        {
            var songs = await _unitOfWork.Songs.GetAllAsync(request.CurrentPage, request.PageSize, cancellationToken);
            var allSongsCount = await _unitOfWork.Songs.CountAsync(cancellationToken);

            return songs.GetPageResponse<Song, SongDto>(allSongsCount, request, _mapper);
        }

        public async Task<PageResponse<SongDto>> GetSongsByTitleAsync(GetPageRequest request, string title, CancellationToken cancellationToken = default)
        {
            var specification = new SongByTitleSpecification(title);
            var songs = await _unitOfWork.Songs.ApplySpecificationAsync(specification, request.CurrentPage, request.PageSize, cancellationToken);
            var allSongsCount = await _unitOfWork.Songs.CountAsync(specification, cancellationToken);

            return songs.GetPageResponse<Song, SongDto>(allSongsCount, request, _mapper);
        }

        public async Task<PageResponse<SongDto>> GetSongsFromGenreAsync(GetPageRequest request, 
            string genreName, 
            CancellationToken cancellationToken = default)
        {
            var specification = new SongFromGenreSpecification(genreName);
            var songs = await _unitOfWork.Songs.ApplySpecificationAsync(specification, request.CurrentPage, request.PageSize, cancellationToken);
            var allSongsCount = await _unitOfWork.Songs.CountAsync(specification, cancellationToken);

            return songs.GetPageResponse<Song, SongDto>(allSongsCount, request, _mapper);
        }

        public async Task<SongDto> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var song = await GetDomainSongAsync(id, cancellationToken);

            return _mapper.Map<SongDto>(song);
        }

        public async Task<SongDto> UpdateAsync(string id, UpdateSongRequest request, CancellationToken cancellationToken = default)
        {
            var song = await GetDomainSongAsync(id, cancellationToken);
            _mapper.Map(request, song);
            await _sourceRepository.SourceExistsAsync(song.SourceName);

            if (request.Genres != null) { await UpdateSongGenresAsync(song, request.Genres, cancellationToken); }

            _unitOfWork.Songs.Update(song);
            await _unitOfWork.CommitAsync(cancellationToken);

            return _mapper.Map<SongDto>(song);
        }

        public async Task UploadSongSourceAsync(ClaimsPrincipal user, 
            UploadSongSourceRequest request, 
            CancellationToken cancellationToken = default)
        {
            var userName = user.Identity!.Name!;
            var authorName = await GetAuthorNameAsync(userName);

            if (!user.IsInRole(UserRoles.admin)) { await CheckIfUserIsMemberAsync(authorName, userName, cancellationToken); }
            
            using var stream = new MemoryStream();
            await request.sourceFile.CopyToAsync(stream, cancellationToken);
            stream.Position = 0;
            await _sourceRepository.UploadAsync(authorName, request.sourceFile.FileName, stream, cancellationToken);
        }

        public async Task RemoveSongSourceAsync(ClaimsPrincipal user, string sourceName, CancellationToken cancellationToken = default)
        {
            var userName = user.Identity!.Name!;
            var authorName = await GetAuthorNameAsync(userName);

            if (!user.IsInRole(UserRoles.admin)) { await CheckIfUserIsMemberAsync(authorName, user.Identity!.Name!, cancellationToken); }
            
            await _sourceRepository.RemoveAsync(authorName, sourceName, cancellationToken);
        }

        public async Task<IEnumerable<string>> GetSourcesAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            var userName = user.Identity!.Name!;
            var authorName = await GetAuthorNameAsync(userName);

            if (!user.IsInRole(UserRoles.admin)) { await CheckIfUserIsMemberAsync(authorName, user.Identity!.Name!, cancellationToken); }

            return await _sourceRepository.GetFromPrefixAsync(authorName, cancellationToken);
        }

        public async Task<MemoryStream> GetSourceStreamAsync(ClaimsPrincipal user, 
            string authorName,
            string sourceName,
            RangeItemHeaderValue? range = null,
            CancellationToken cancellationToken = default)
        {
            long offset = range?.From ?? 0;
            long length = range?.To - offset + 1 ?? 0;

            return await _sourceRepository.GetSourceStream(authorName, sourceName, offset, length, cancellationToken);
        }

        private async Task<string> GetAuthorNameAsync(string userName)
        {
            var user = await _unitOfWork.Users.GetByUserNameAsync(userName);

            if (user == null)
            {
                throw new NotFoundException(ExceptionMessages.UserNotFound);
            }

            if (user.Author == null)
            {
                throw new NotFoundException(ExceptionMessages.AuthorNotFound);
            }

            return user.Author!.Name;
        }

        private async Task<Song> GetDomainSongAsync(string id, CancellationToken cancellationToken = default)
        {
            var song = await _unitOfWork.Songs.GetByIdAsync(id, cancellationToken);

            if (song == null)
            {
                throw new NotFoundException(ExceptionMessages.SongNotFound);
            }

            return song;
        }

        private async Task UpdateSongGenresAsync(Song song, List<string> genreNames, CancellationToken cancellationToken = default)
        {
            List<Genre> genres = new List<Genre>();

            foreach (var genreName in genreNames)
            {
                var genre = await _unitOfWork.Genres.GetOrCreateAsync(genreName, cancellationToken);
                genres.Add(genre);
            }

            song.Genres = genres;
        }

        private async Task CheckIfUserIsMemberAsync(string authorName, string userName, CancellationToken cancellationToken)
        {
            var author = await _unitOfWork.Authors.GetByNameAsync(authorName, cancellationToken);

            if (author == null)
            {
                throw new NotFoundException(ExceptionMessages.AuthorNotFound);
            }

            if (!_unitOfWork.Authors.UserIsMember(author, userName))
            {
                throw new AuthorizationException(ExceptionMessages.NotAuthorMember);
            }
        }
    }
}
