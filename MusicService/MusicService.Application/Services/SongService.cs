using AutoMapper;
using MusicService.Application.Interfaces;
using MusicService.Application.Models;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.SongService;
using MusicService.Domain.Entities;
using MusicService.Domain.Exceptions;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Extensions;

namespace MusicService.Application.Services
{
    public class SongService : ISongService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SongService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task ChangeGenreDescriptionAsync(string name, ChangeGenreDescriptionRequest request)
        {
            var genre = await GetDomainGenreAsync(name);
            genre.Description = request.NewDescription;
            await _unitOfWork.CommitAsync();
        }

        public async Task ChangeSongSourceAsync(string id, ChangeSongSourceRequest request)
        {
            var song = await GetDomainSongAsync(id);
            song.SourceName = request.NewSource;
            await _unitOfWork.CommitAsync();
        }

        public Task CheckIfSourceExists(string source)
        {
            //TODO: implement it after adding MinIO

            return Task.FromResult(true);
        }

        public async Task<PageResponse<SongDto>> GetAllAsync(GetPageRequest request)
        {
            var songs = await _unitOfWork.Songs.GetAllAsync(request.CurrentPage, request.PageSize);
            var allSongsCount = await _unitOfWork.Songs.CountAsync();

            return songs.GetPageResponse<Song, SongDto>(allSongsCount, request, _mapper);
        }

        public async Task<SongDto> GetByIdAsync(string id)
        {
            var song = await GetDomainSongAsync(id);

            return _mapper.Map<SongDto>(song);
        }

        public async Task UpdateAsync(string id, UpdateSongRequest request)
        {
            var song = await GetDomainSongAsync(id);
            _mapper.Map(request, song);
            List<Genre> genres = new List<Genre>();

            foreach(var genreName in request.Genres) 
            {
                var genre = await _unitOfWork.Genres.GetOrCreateAsync(genreName);
                genres.Add(genre);
            }

            song.Genres = genres;
            await _unitOfWork.CommitAsync();
        }

        public async Task<PageResponse<GenreDto>> GetAllGenresAsync(GetPageRequest request)
        {
            var genres = await _unitOfWork.Genres.GetAllAsync(request.CurrentPage, request.PageSize);
            var allGenresCount = await _unitOfWork.Genres.CountAsync();

            return genres.GetPageResponse<Genre, GenreDto>(allGenresCount, request, _mapper);
        }

        public async Task<GenreDto> GetGenreByNameAsync(string name)
        {
            var genre = await GetDomainGenreAsync(name);

            return _mapper.Map<GenreDto>(genre);
        }

        public async Task DeleteEmptyGenres()
        {
            _unitOfWork.Genres.DeleteAllEmpty();
            await _unitOfWork.CommitAsync();
        }

        private async Task<Song> GetDomainSongAsync(string id)
        {
            var song = await _unitOfWork.Songs.GetByIdAsync(id);

            if (song == null)
            {
                throw new NotFoundException("No song with such id was found.");
            }

            return song;
        }

        private async Task<Genre> GetDomainGenreAsync(string name)
        {
            var genre = await _unitOfWork.Genres.GetByNameAsync(name);

            if (genre == null)
            {
                throw new NotFoundException("No genre was found.");
            }

            return genre;
        }
    }
}
