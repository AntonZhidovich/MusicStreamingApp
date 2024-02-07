using Microsoft.AspNetCore.Mvc;
using MusicService.Application.Models.SongService;
using MusicService.Application.Models;
using MusicService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MusicService.API.Controllers
{
    [Authorize]
    [Route("api/genres")]
    [ApiController]
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
        public async Task<IActionResult> ChangeGenreDescriptionAsync([FromRoute] string name)
        {
            var genre = await _songService.GetGenreByNameAsync(name);

            return Ok(genre);
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> ChangeGenreDescriptionAsync([FromRoute] string name, [FromBody] ChangeGenreDescriptionRequest request)
        {
            await _songService.ChangeGenreDescriptionAsync(name, request);

            return NoContent();
        }
    }
}
