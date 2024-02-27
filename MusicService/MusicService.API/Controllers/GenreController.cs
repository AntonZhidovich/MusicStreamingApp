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
        private readonly IGenreService _songService;

        public GenreController(IGenreService songService)
        {
            _songService = songService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllGenresAsync([FromQuery] GetPageRequest request)
        {
            var genresPage = await _songService.GetAllAsync(request, HttpContext.RequestAborted);

            return Ok(genresPage);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> ChangeGenreDescriptionAsync([FromRoute] string name)
        {
            var genre = await _songService.GetByNameAsync(name, HttpContext.RequestAborted);

            return Ok(genre);
        }

        [HttpPut("{name}")]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> ChangeGenreDescriptionAsync([FromRoute] string name, [FromBody] UpdateGenreRequest request)
        {
            var genre = await _songService.UpdateAsync(name, request, HttpContext.RequestAborted);

            return Ok(genre);
        }

        [HttpDelete("{name}")]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string name)
        {
            await _songService.DeleteAsync(name, HttpContext.RequestAborted);

            return NoContent();
        }
    }
}
