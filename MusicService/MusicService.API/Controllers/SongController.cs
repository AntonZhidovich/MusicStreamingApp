using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicService.Application.Interfaces;
using MusicService.Application.Models;
using MusicService.Application.Models.SongService;
using MusicService.Domain.Constants;

namespace MusicService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/songs")]
    public class SongController : ControllerBase
    {
        private readonly ISongService _songService;
        private const string contentType = "audio/mpeg";

        public SongController(ISongService songService)
        {
            _songService = songService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetPageRequest request)
        {
            var songs = await _songService.GetAllAsync(request, HttpContext.RequestAborted);

            return Ok(songs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
        {
            var songs = await _songService.GetByIdAsync(id, HttpContext.RequestAborted);

            return Ok(songs);
        }

        [HttpGet("genre/{genreName}")]
        public async Task<IActionResult> GetSongsFromGenreAsync([FromRoute] string genreName, [FromQuery] GetPageRequest request)
        {
            var songs = await _songService.GetSongsFromGenreAsync(request, genreName, HttpContext.RequestAborted);

            return Ok(songs);
        }

        [HttpGet("title/{title}")]
        public async Task<IActionResult> GetSongsByNameAsync([FromRoute] string title, [FromQuery] GetPageRequest request)
        {
            var songs = await _songService.GetSongsByTitleAsync(request, title, HttpContext.RequestAborted);

            return Ok(songs);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> UpdateByIdASync([FromRoute] string id, [FromBody] UpdateSongRequest request)
        {
            var updatedSong = await _songService.UpdateAsync(id, request, HttpContext.RequestAborted);

            return Ok(updatedSong);
        }

        [HttpPost("sources")]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> UploadSourceAsync([FromForm] UploadSongSourceRequest request)
        {
            await _songService.UploadSongSourceAsync(User, request, HttpContext.RequestAborted);

            return NoContent();
        }

        [HttpGet("sources/")]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> GetSourcesAsync()
        {
            var sources = await _songService.GetSourcesAsync(User, HttpContext.RequestAborted);

            return Ok(sources);
        }

        [HttpDelete("sources/{sourceName}")]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> DeleteSourceAsync([FromRoute] string sourceName)
        {
            await _songService.RemoveSongSourceAsync(User, sourceName, HttpContext.RequestAborted);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("sources/{authorName}/{sourceName}")]
        public async Task<IActionResult> GetSourceAsync([FromRoute] string authorName, [FromRoute] string sourceName)
        {
            var rangeHeader = Request.GetTypedHeaders().Range?.Ranges.First();
            var memoryStream = await _songService.GetSourceStreamAsync(User, authorName, sourceName, rangeHeader, HttpContext.RequestAborted);

            return File(memoryStream, contentType, true);
        }
    }
}
