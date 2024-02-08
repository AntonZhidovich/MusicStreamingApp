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

        public SongController(ISongService songService)
        {
            _songService = songService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetPageRequest request)
        {
            var songs = await _songService.GetAllAsync(request);

            return Ok(songs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
        {
            var songs = await _songService.GetByIdAsync(id);

            return Ok(songs);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{UserRoles.creator},{UserRoles.admin}")]
        public async Task<IActionResult> UpdateByIdASync([FromRoute] string id, [FromBody] UpdateSongRequest request)
        {
            await _songService.UpdateAsync(id, request);

            return NoContent();
        }

        [HttpPut("{id}/source")]
        [Authorize(Roles = UserRoles.admin)]
        public async Task<IActionResult> ChangeSourceAsync([FromRoute] string id, [FromBody] ChangeSongSourceRequest request)
        {
            await _songService.ChangeSongSourceAsync(id, request);

            return NoContent();
        }
    }
}
