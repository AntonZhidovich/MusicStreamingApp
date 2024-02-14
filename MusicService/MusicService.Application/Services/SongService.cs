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

        public async Task ChangeGenreDescriptionAsync(string name, 
            ChangeGenreDescriptionRequest request, 
            CancellationToken cancellationToken = default)
        {
            var genre = await GetDomainGenreAsync(name, cancellationToken);
            genre.Description = request.NewDescription;
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        public async Task DeleteGenreAsync(string name, CancellationToken cancellationToken = default)
        {
            var genre = await GetDomainGenreAsync(name);
            _unitOfWork.Genres.Delete(genre);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

            public async Task CheckIfSourceExistsAsync(string source, CancellationToken cancellationToken = default)
            {
            await _sourceRepository.SourceExistsAsync(source);
        }

        public async Task<PageResponse<SongDto>> GetAllAsync(GetPageRequest request, CancellationToken cancellationToken = default)
        {
            var songs = await _unitOfWork.Songs.GetAllAsync(request.CurrentPage, request.PageSize, cancellationToken);
            var allSongsCount = await _unitOfWork.Songs.CountAsync(cancellationToken);

            return songs.GetPageResponse<Song, SongDto>(allSongsCount, request, _mapper);
        }

        public async Task<PageResponse<SongDto>> GetSongsByNameAsync(GetPageRequest request, string name, CancellationToken cancellationToken = default)
        {
            var specification = new SongFromGenreSpecification(name);
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

        public async Task UpdateAsync(string id, UpdateSongRequest request, CancellationToken cancellationToken = default)
        {
            var song = await GetDomainSongAsync(id, cancellationToken);
            _mapper.Map(request, song);

            if (request.Genres != null) { await UpdateSongGenresAsync(song, request.Genres, cancellationToken); }

            await _unitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<PageResponse<GenreDto>> GetAllGenresAsync(GetPageRequest request, CancellationToken cancellationToken = default)
        {
            var genres = await _unitOfWork.Genres.GetAllAsync(request.CurrentPage, request.PageSize, cancellationToken);
            var allGenresCount = await _unitOfWork.Genres.CountAsync(cancellationToken);

            return genres.GetPageResponse<Genre, GenreDto>(allGenresCount, request, _mapper);
        }

        public async Task<GenreDto> GetGenreByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var genre = await GetDomainGenreAsync(name, cancellationToken);

            return _mapper.Map<GenreDto>(genre);
        }

        public async Task UploadSongSourceAsync(ClaimsPrincipal user, UploadSongSourceRequest request, CancellationToken cancellationToken = default)
        {
            if (!user.IsInRole(UserRoles.admin)) { await CheckIfUserIsMemberAsync(request.AuthorName, user.Identity!.Name!, cancellationToken); }
            
            using var stream = new MemoryStream();
            await request.sourceFile.CopyToAsync(stream, cancellationToken);
            stream.Position = 0;
            await _sourceRepository.UploadAsync(request.AuthorName, request.sourceFile.FileName, stream, cancellationToken);
        }

        public async Task RemoveSongSourceAsync(ClaimsPrincipal user, string authorName, string sourceName, CancellationToken cancellationToken = default)
        {
            if (!user.IsInRole(UserRoles.admin)) { await CheckIfUserIsMemberAsync(authorName, user.Identity!.Name!, cancellationToken); }
            
            await _sourceRepository.RemoveAsync(authorName, sourceName, cancellationToken);
        }

        public async Task<IEnumerable<string>> GetSourcesAsync(ClaimsPrincipal user, string authorName, CancellationToken cancellationToken = default)
        {
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

        private async Task<Song> GetDomainSongAsync(string id, CancellationToken cancellationToken = default)
        {
            var song = await _unitOfWork.Songs.GetByIdAsync(id, cancellationToken);

            if (song == null)
            {
                throw new NotFoundException("No song with such id was found.");
            }

            return song;
        }

        private async Task<Genre> GetDomainGenreAsync(string name, CancellationToken cancellationToken = default)
        {
            var genre = await _unitOfWork.Genres.GetByNameAsync(name, cancellationToken);

            if (genre == null)
            {
                throw new NotFoundException("No genre was found.");
            }

            return genre;
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
                throw new NotFoundException("No author was found.");
            }

            if (!_unitOfWork.Authors.UserIsMember(author, userName))
            {
                throw new AuthorizationException("Only author members can do this action.");
            }
        }
    }
}
