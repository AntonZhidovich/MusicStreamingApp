using AutoMapper;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models;
using MusicService.Domain.Entities;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Extensions;
using MusicService.Domain.Exceptions;
using MusicService.Application.Models.SongService;
using MusicService.Application.Interfaces;
using MusicService.Domain.Constants;

namespace MusicService.Application.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PageResponse<GenreDto>> GetAllAsync(GetPageRequest request, CancellationToken cancellationToken = default)
        {
            var genres = await _unitOfWork.Genres.GetAllAsync(request.CurrentPage, request.PageSize, cancellationToken);
            var allGenresCount = await _unitOfWork.Genres.CountAsync(cancellationToken);

            return genres.GetPageResponse<Genre, GenreDto>(allGenresCount, request, _mapper);
        }

        public async Task<GenreDto> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var genre = await GetDomainGenreAsync(name, cancellationToken);

            return _mapper.Map<GenreDto>(genre);
        }

        public async Task<GenreDto> UpdateAsync(string name,
            UpdateGenreRequest request,
            CancellationToken cancellationToken = default)
        {
            var genre = await GetDomainGenreAsync(name, cancellationToken);
            genre.Description = request.NewDescription;
            _unitOfWork.Genres.Update(genre);
            await _unitOfWork.CommitAsync(cancellationToken);

            return _mapper.Map<GenreDto>(genre);
        }

        public async Task DeleteAsync(string name, CancellationToken cancellationToken = default)
        {
            var genre = await GetDomainGenreAsync(name);
            _unitOfWork.Genres.Delete(genre);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        private async Task<Genre> GetDomainGenreAsync(string name, CancellationToken cancellationToken = default)
        {
            var genre = await _unitOfWork.Genres.GetByNameAsync(name, cancellationToken);

            if (genre == null)
            {
                throw new NotFoundException(ExceptionMessages.GenreNotFound);
            }

            return genre;
        }
    }
}
