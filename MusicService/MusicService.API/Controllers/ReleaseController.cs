using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicService.Application.Interfaces;
using MusicService.Application.Models;
using MusicService.Application.Models.ReleaseService;
using MusicService.Domain.Constants;

namespace MusicService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/releases")]
    public class ReleaseController : ControllerBase
    {
        private readonly IReleaseService _releaseService;

        public ReleaseController(IReleaseService releaseService)
        {
            _releaseService = releaseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetPageRequest request)
        {
            var releases = await _releaseService.GetAllAsync(request);

            return Ok(releases);
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.creator},{UserRoles.admin}")]
        public async Task<IActionResult> CreateAsync(CreateReleaseRequest request)
        {
            await _releaseService.CreateAsync(request, HttpContext.User);

            return NoContent();
        }

        [HttpPatch("{releaseId}")]
        [Authorize(Roles = $"{UserRoles.creator},{UserRoles.admin}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string releaseId, [FromBody] UpdateReleaseRequest request)
        {
            await _releaseService.UpdateAsync(releaseId, request, HttpContext.User);

            return NoContent();
        }

        [HttpDelete("{releaseId}")]
        [Authorize(Roles = $"{UserRoles.creator},{UserRoles.admin}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string releaseId)
        {
            await _releaseService.DeleteAsync(releaseId, HttpContext.User);

            return NoContent();
        }

        [HttpPost("{releaseId}/songs")]
        [Authorize(Roles = $"{UserRoles.creator},{UserRoles.admin}")]
        public async Task<IActionResult> AddSongAsync([FromRoute] string releaseId, [FromBody] AddSongToReleaseRequest request)
        {
            await _releaseService.AddSongToReleaseAsync(releaseId, request, HttpContext.User);

            return NoContent();
        }


        [HttpDelete("{releaseId}/songs/{songId}")]
        [Authorize(Roles = $"{UserRoles.creator},{UserRoles.admin}")]
        public async Task<IActionResult> RemoveSongAsync([FromRoute] string releaseId, [FromRoute] string songId)
        {
            await _releaseService.RemoveSongFromReleaseAsync(releaseId, songId, HttpContext.User);

            return NoContent();
        }
    }
}
