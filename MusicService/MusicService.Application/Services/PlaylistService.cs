using AutoMapper;
using Microsoft.Extensions.Logging;
using MusicService.Application.Interfaces;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.PlaylistService;
using MusicService.Domain.Constants;
using MusicService.Domain.Entities;
using MusicService.Domain.Exceptions;
using MusicService.Domain.Interfaces;
using System.Security.Claims;

namespace MusicService.Application.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly ILogger<PlaylistService> _logger;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlaylistService(
            ILogger<PlaylistService> logger,
            IPlaylistRepository playlistRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _playlistRepository = playlistRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddSongAsync(ClaimsPrincipal user, string playlistId, AddSongToPlaylistRequest request, CancellationToken cancellationToken = default)
        {
            var song = await _unitOfWork.Songs.GetByIdAsync(request.SongId, cancellationToken);

            if (song == null)
            {
                throw new NotFoundException(ExceptionMessages.SongNotFound);
            }

            var playlist = await GetDomainPlaylistAsync(playlistId, cancellationToken);

            if (playlist.SongIds.Contains(song.Id)) 
            { 
                throw new BadRequestException(ExceptionMessages.SongAlreadyInPlaylist);  
            }

            CheckIfUserIsOwner(playlist, GetCurrentUserId(user));
            playlist.SongIds.Add(song.Id);
            
            await _playlistRepository.UpdateAsync(playlist, cancellationToken);
        }

        public async Task RemoveSongAsync(ClaimsPrincipal user, string playlistId, string songId, CancellationToken cancellationToken = default)
        {
            var playlist = await GetDomainPlaylistAsync(playlistId, cancellationToken);
            
            CheckIfUserIsOwner(playlist, GetCurrentUserId(user));

            if (!playlist.SongIds.Contains(songId))
            {
                throw new NotFoundException(ExceptionMessages.SongNotFound);
            }

            playlist.SongIds.Remove(songId);
            
            await _playlistRepository.UpdateAsync(playlist, cancellationToken);
        }

        public async Task CreateAsync(ClaimsPrincipal user, CreatePlaylistRequest request, CancellationToken cancellationToken = default)
        {
            var userId = GetCurrentUserId(user);
            
            var maxPlaylistCount = await GetUserMaxPlaylistCountAsync(userId, cancellationToken);
            var currentPlaylistCount = await _playlistRepository.CountAsync(userId, cancellationToken);

            if (currentPlaylistCount >= maxPlaylistCount) 
            {
                _logger.LogError("Tariff plan of user {userId} did't allow to create another playlist. Current count: {currentCount}; Max count: {maxCount}.",
                    userId, currentPlaylistCount, maxPlaylistCount);

                throw new BadRequestException(ExceptionMessages.PlanDoesntAllow);
            }

            var playlist = new Playlist { UserId = userId };
            _mapper.Map(request, playlist);

            _logger.LogInformation("User {userId} created a playlist {playlitId}.", userId, playlist.Id);
            
            await _playlistRepository.CreateAsync(playlist, cancellationToken);
        }

        public async Task DeleteAsync(ClaimsPrincipal user, string id, CancellationToken cancellationToken = default)
        {
            var playlist = await GetDomainPlaylistAsync(id, cancellationToken);
            
            CheckIfUserIsOwner(playlist, GetCurrentUserId(user));

            await _playlistRepository.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("Playlist {playlitId} is deleted.", playlist.Id);
        }

        public async Task<PlaylistFullDto> GetFullPlaylistAsync(ClaimsPrincipal user, string id, CancellationToken cancellationToken = default)
        {
            var playlist = await GetDomainPlaylistAsync(id, cancellationToken);
            
            CheckIfUserIsOwner(playlist, GetCurrentUserId(user));

            var songs = await _unitOfWork.Songs.GetByIdAsync(playlist.SongIds, cancellationToken);

            var playlistDto = _mapper.Map<PlaylistFullDto>(playlist);
            playlistDto.Songs = _mapper.Map<List<SongDto>>(songs);

            if (playlistDto.Songs.Count != playlist.SongIds.Count)
            {
                var foundSongsId = songs.Select(song => song.Id);
                var notFoundSongsId = playlist.SongIds.Where(songId => !foundSongsId.Contains(songId)).ToList();
                playlistDto.Songs.AddRange(notFoundSongsId.Select(id => new SongDto { Id = id, Title = "Unavailiable" }));
            }

            return playlistDto;
        }

        public async Task<IEnumerable<PlaylistShortDto>> GetUserPlaylistsAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            var userId = GetCurrentUserId(user);
            
            var maxPlaylistCount = await GetUserMaxPlaylistCountAsync(userId, cancellationToken);
            var playlists = await _playlistRepository.GetUserPlaylistsAsync(userId, maxPlaylistCount, cancellationToken);

            return _mapper.Map<IEnumerable<PlaylistShortDto>>(playlists);
        }

        public async Task UpdateAsync(ClaimsPrincipal user, string id, UpdatePlaylistRequest request, CancellationToken cancellationToken = default)
        {
            var playlist = await GetDomainPlaylistAsync(id, cancellationToken);

            CheckIfUserIsOwner(playlist, GetCurrentUserId(user));
            playlist.Name = request.Name;
            
            await _playlistRepository.UpdateAsync(playlist);
        }

        private void CheckIfUserIsOwner(Playlist playlist, string userId)
        {
            if (playlist.UserId != userId)
            {
                _logger.LogError("User {userId} attempted to access a playlist {playlistId} he doesn't owe.", userId, playlist.Id);

                throw new AuthorizationException(ExceptionMessages.NoAccessToPlaylist);
            }
        }

        private async Task<int> GetUserMaxPlaylistCountAsync(string userId, CancellationToken cancellationToken = default)
        {
            var count = await _playlistRepository.GetUserMaxPlaylistCountAsync(userId, cancellationToken);

            if (count == null)
            {
                _logger.LogError("Tariff plan for user {userId} is not specified.", userId);

                throw new BadRequestException(ExceptionMessages.PlanNotFound);
            }

            return count.Value;
        }

        private async Task<Playlist> GetDomainPlaylistAsync(string id, CancellationToken cancellationToken = default)
        {
            var playlist = await _playlistRepository.GetAsync(id, cancellationToken);

            if (playlist == null)
            {
                _logger.LogError("Playist with id {playlistId} was not found.", id);

                throw new NotFoundException(ExceptionMessages.PlaylistNotFound);
            }

            return playlist;
        }

        private string GetCurrentUserId(ClaimsPrincipal user)
        {
            return user.Claims
                .Where(claim => claim.Type == ClaimTypes.NameIdentifier)
                .Select(claim => claim.Value)
                .FirstOrDefault()!;
        }
    }
}
