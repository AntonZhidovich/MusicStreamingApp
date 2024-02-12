using AutoMapper;
using MusicService.Application.Interfaces;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.PlaylistService;
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
                throw new NotFoundException("No song was found.");
            }

            var playlist = await GetDomainPlaylistAsync(playlistId, cancellationToken);

            if (playlist.SongIds.Contains(song.Id)) 
            { 
                throw new BadRequestException("Song is already in the playlist.");  
            }

            if (playlist.UserName != user.Identity!.Name!)
            {
                throw new AuthorizationException("Authorized user doesn't have acces to this playlist.");
            }

            playlist.SongIds.Add(song.Id);
            await _playlistRepository.UpdateAsync(playlist, cancellationToken);
        }

        public async Task RemoveSongAsync(ClaimsPrincipal user, string playlistId, string songId, CancellationToken cancellationToken = default)
        {
            var playlist = await GetDomainPlaylistAsync(playlistId, cancellationToken);

            if (playlist.UserName != user.Identity!.Name!)
            {
                throw new AuthorizationException("Authorized user doesn's acces to this playlist.");
            }

            if (!playlist.SongIds.Contains(songId))
            {
                throw new NotFoundException("No song was found in the playlist.");
            }

            playlist.SongIds.Remove(songId);
            await _playlistRepository.UpdateAsync(playlist, cancellationToken);
        }

        public async Task CreateAsync(ClaimsPrincipal user, CreatePlaylistRequest request, CancellationToken cancellationToken = default)
        {
            var playlist = new Playlist { UserName = user.Identity!.Name! };
            _mapper.Map(request, playlist);
            await _playlistRepository.CreateAsync(playlist, cancellationToken);
        }

        public async Task DeleteAsync(ClaimsPrincipal user, string id, CancellationToken cancellationToken = default)
        {
            var playlist = await GetDomainPlaylistAsync(id, cancellationToken);

            if (playlist.UserName != user.Identity!.Name!)
            {
                throw new AuthorizationException("Authorized user doesn's acces to this playlist.");
            }

            await _playlistRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task<FullPlaylistDto> GetFullPlaylistAsync(ClaimsPrincipal user, string id, CancellationToken cancellationToken = default)
        {
            var playlist = await GetDomainPlaylistAsync(id, cancellationToken);

            if (playlist.UserName != user.Identity!.Name!)
            {
                throw new AuthorizationException("Authorized user doesn's acces to this playlist.");
            }

            var songs = await _unitOfWork.Songs.GetByIdAsync(playlist.SongIds, cancellationToken);

            var playlistDto = _mapper.Map<FullPlaylistDto>(playlist);
            playlistDto.Songs = _mapper.Map<List<SongDto>>(songs);

            if (playlistDto.Songs.Count != playlist.SongIds.Count)
            {
                var foundSongsId = songs.Select(song => song.Id);
                var notFoundSongsId = playlist.SongIds.Where(songId => !foundSongsId.Contains(songId)).ToList();
                playlistDto.Songs.AddRange(notFoundSongsId.Select(id => new SongDto { Id = id, Title = "Unavailiable" }));
            }

            return playlistDto;
        }

        public async Task<IEnumerable<ShortPlaylistDto>> GetUserPlaylistsAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            var playlists = await _playlistRepository.GetUserPlaylistsAsync(user.Identity!.Name!, cancellationToken);

            return _mapper.Map<IEnumerable<ShortPlaylistDto>>(playlists);
        }

        public async Task UpdateAsync(ClaimsPrincipal user, string id, UpdatePlaylistRequest request, CancellationToken cancellationToken = default)
        {
            var playlist = await GetDomainPlaylistAsync(id, cancellationToken);
            playlist.Name = request.Name;
            await _playlistRepository.UpdateAsync(playlist);
        }

        private async Task<Playlist> GetDomainPlaylistAsync(string id, CancellationToken cancellationToken = default)
        {
            var playlist = await _playlistRepository.GetAsync(id, cancellationToken);

            if (playlist == null)
            {
                throw new NotFoundException("No playlits was found.");
            }

            return playlist;
        }
    }
}
