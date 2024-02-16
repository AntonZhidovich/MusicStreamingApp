using AutoMapper;
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
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlaylistService(
            IPlaylistRepository playlistRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
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

            CheckIfUserIsOwnerAsync(playlist, user.Identity!.Name!);
            playlist.SongIds.Add(song.Id);
            
            await _playlistRepository.UpdateAsync(playlist, cancellationToken);
        }

        public async Task RemoveSongAsync(ClaimsPrincipal user, string playlistId, string songId, CancellationToken cancellationToken = default)
        {
            var playlist = await GetDomainPlaylistAsync(playlistId, cancellationToken);
            
            CheckIfUserIsOwnerAsync(playlist, user.Identity!.Name!);

            if (!playlist.SongIds.Contains(songId))
            {
                throw new NotFoundException(ExceptionMessages.SongNotFound);
            }

            playlist.SongIds.Remove(songId);
            
            await _playlistRepository.UpdateAsync(playlist, cancellationToken);
        }

        public async Task CreateAsync(ClaimsPrincipal user, CreatePlaylistRequest request, CancellationToken cancellationToken = default)
        {
            var userName = user.Identity!.Name!;
            
            var maxPlaylistCount = await GetUserMaxPlaylistCountAsync(userName, cancellationToken);
            var currentPlaylistCount = await _playlistRepository.CountAsync(userName, cancellationToken);

            if (currentPlaylistCount >= maxPlaylistCount) 
            {
                throw new BadRequestException(ExceptionMessages.PlanDoesntAllow);
            }

            var playlist = new Playlist { UserName = userName };
            _mapper.Map(request, playlist);
            
            await _playlistRepository.CreateAsync(playlist, cancellationToken);
        }

        public async Task DeleteAsync(ClaimsPrincipal user, string id, CancellationToken cancellationToken = default)
        {
            var playlist = await GetDomainPlaylistAsync(id, cancellationToken);
            CheckIfUserIsOwnerAsync(playlist, user.Identity!.Name!);

            await _playlistRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task<PlaylistFullDto> GetFullPlaylistAsync(ClaimsPrincipal user, string id, CancellationToken cancellationToken = default)
        {
            var playlist = await GetDomainPlaylistAsync(id, cancellationToken);
            
            CheckIfUserIsOwnerAsync(playlist, user.Identity!.Name!);

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
            var userName = user.Identity!.Name!;
            
            var maxPlaylistCount = await GetUserMaxPlaylistCountAsync(userName, cancellationToken);
            var playlists = await _playlistRepository.GetUserPlaylistsAsync(userName, maxPlaylistCount, cancellationToken);

            return _mapper.Map<IEnumerable<PlaylistShortDto>>(playlists);
        }

        public async Task UpdateAsync(ClaimsPrincipal user, string id, UpdatePlaylistRequest request, CancellationToken cancellationToken = default)
        {
            var playlist = await GetDomainPlaylistAsync(id, cancellationToken);
            playlist.Name = request.Name;
            
            await _playlistRepository.UpdateAsync(playlist);
        }

        private void CheckIfUserIsOwnerAsync(Playlist playlist, string userName)
        {
            if (playlist.UserName != userName)
            {
                throw new AuthorizationException(ExceptionMessages.NoAccessToPlaylist);
            }
        }

        private async Task<int> GetUserMaxPlaylistCountAsync(string userName, CancellationToken cancellationToken = default)
        {
            var count = await _playlistRepository.GetUserMaxPlaylistCountAsync(userName, cancellationToken);

            if (count == null)
            {
                throw new BadRequestException(ExceptionMessages.PlanNotFound);
            }

            return count.Value;
        }

        private async Task<Playlist> GetDomainPlaylistAsync(string id, CancellationToken cancellationToken = default)
        {
            var playlist = await _playlistRepository.GetAsync(id, cancellationToken);

            if (playlist == null)
            {
                throw new NotFoundException(ExceptionMessages.PlaylistNotFound);
            }

            return playlist;
        }
    }
}
