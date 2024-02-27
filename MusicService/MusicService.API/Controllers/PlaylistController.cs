using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicService.Application.Interfaces;
using MusicService.Application.Models.PlaylistService;

namespace MusicService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/playlists")]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPlaylists()
        {
            var playlists = await _playlistService.GetUserPlaylistsAsync(User, HttpContext.RequestAborted);

            return Ok(playlists);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]CreatePlaylistRequest request)
        {
            await _playlistService.CreateAsync(User, request, HttpContext.RequestAborted);

            return NoContent();
        }

        [HttpPost("{playlistId}/songs")]
        public async Task<IActionResult> AddSongAsync([FromRoute] string playlistId, [FromBody] AddSongToPlaylistRequest request)
        {
            await _playlistService.AddSongAsync(User, playlistId, request, HttpContext.RequestAborted);

            return NoContent();
        }

        [HttpGet("{playlistId}/songs")]
        public async Task<IActionResult> GetFullPlaylistAsync([FromRoute] string playlistId)
        {
            var playlist = await _playlistService.GetFullPlaylistAsync(User, playlistId, HttpContext.RequestAborted);

            return Ok(playlist);
        }

        [HttpDelete("{playlistId}/songs/{songId}")]
        public async Task<IActionResult> RemoveSongAsync([FromRoute] string playlistId, [FromRoute] string songId)
        {
            await _playlistService.RemoveSongAsync(User, playlistId, songId, HttpContext.RequestAborted);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            await _playlistService.DeleteAsync(User, id, HttpContext.RequestAborted);

            return NoContent();
        }
    }
}
