using Microsoft.AspNetCore.Mvc;
using MusicService.Application.Models.SongService;
using MusicService.Application.Models;
using MusicService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using MusicService.Domain.Constants;

namespace MusicService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/genres")]
    public class GenreController : ControllerBase
    {
        private readonly ISongService _songService;

        public GenreController(ISongService songService)
        {
            _songService = songService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllGenresAsync([FromQuery] GetPageRequest request)
        {
            var genresPage = await _songService.GetAllGenresAsync(request);

            return Ok(genresPage);
        }

        [HttpGet("{name}")]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> ChangeGenreDescriptionAsync([FromRoute] string name)
        {
            var genre = await _songService.GetGenreByNameAsync(name);

            return Ok(genre);
        }

        [HttpPut("{name}")]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> ChangeGenreDescriptionAsync([FromRoute] string name, [FromBody] ChangeGenreDescriptionRequest request)
        {
            await _songService.ChangeGenreDescriptionAsync(name, request);

            return NoContent();
        }

        [HttpDelete("{name}")]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string name)
        {
            await _songService.DeleteGenreAsync(name);

            return NoContent();
        }
    }
}
