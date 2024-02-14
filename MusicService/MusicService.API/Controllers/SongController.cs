using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicService.Application.Interfaces;
using MusicService.Application.Models;
using MusicService.Application.Models.SongService;
using MusicService.Domain.Constants;

namespace MusicService.API.Controllers
{
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

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetSongsByNameAsync([FromRoute] string name, [FromQuery] GetPageRequest request)
        {
            var songs = await _songService.GetSongsByNameAsync(request, name, HttpContext.RequestAborted);

            return Ok(songs);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{UserRoles.admin}")]
        public async Task<IActionResult> UpdateByIdASync([FromRoute] string id, [FromBody] UpdateSongRequest request)
        {
            await _songService.UpdateAsync(id, request, HttpContext.RequestAborted);

            return NoContent();
        }

        [HttpPost("source")]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> UploadSourceAsync([FromForm] UploadSongSourceRequest request)
        {
            await _songService.UploadSongSourceAsync(User, request, HttpContext.RequestAborted);

            return NoContent();
        }

        [HttpGet("source/{authorName}")]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> RemoveSourceAsync([FromRoute] string authorName)
        {
            var sources = await _songService.GetSourcesAsync(User, authorName, HttpContext.RequestAborted);

            return Ok(sources);
        }

        [AllowAnonymous]
        [HttpGet("source/{authorName}/{sourceName}")]
        public async Task<IActionResult> GetSourceAsync([FromRoute] string authorName, [FromRoute] string sourceName)
        {
            var rangeHeader = Request.GetTypedHeaders().Range?.Ranges.First();
            var memoryStream = await _songService.GetSourceStreamAsync(User, authorName, sourceName, rangeHeader, HttpContext.RequestAborted);

            return File(memoryStream, contentType, true);
        }
    }
}
