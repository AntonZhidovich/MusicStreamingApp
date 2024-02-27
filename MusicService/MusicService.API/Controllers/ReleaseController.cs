﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicService.Application.Interfaces;
using MusicService.Application.Models;
using MusicService.Application.Models.ReleaseService;
using MusicService.Domain.Constants;
using MusicService.Domain.Interfaces;

namespace MusicService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/releases")]
    public class ReleaseController : ControllerBase
    {
        private readonly IReleaseService _releaseService;

        public ReleaseController(IReleaseService releaseService, ISongSourceRepository repo)
        {
            _releaseService = releaseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetPageRequest request)
        {
            var releases = await _releaseService.GetAllAsync(request, HttpContext.RequestAborted);

            return Ok(releases);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
        {
            var release = await _releaseService.GetByIdAsync(id);

            return Ok(release);
        }

        [HttpGet("author/{authorName}")]
        public async Task<IActionResult> GetAllFromAuthorAsync([FromRoute] string authorName, [FromQuery] GetPageRequest request)
        {
            var authors = await _releaseService.GetAllFromAuthorAsync(request, authorName);

            return Ok(authors);
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.creator},{UserRoles.admin}")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateReleaseRequest request)
        {
            var release = await _releaseService.CreateAsync(request, HttpContext.User, HttpContext.RequestAborted);

            return Ok(release);
        }

        [HttpPut("{releaseId}")]
        [Authorize(Roles = $"{UserRoles.creator},{UserRoles.admin}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string releaseId, [FromBody] UpdateReleaseRequest request)
        {
            var release = await _releaseService.UpdateAsync(releaseId, request, HttpContext.User, HttpContext.RequestAborted);

            return Ok(release);
        }

        [HttpDelete("{releaseId}")]
        [Authorize(Roles = $"{UserRoles.creator},{UserRoles.admin}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string releaseId)
        {
            await _releaseService.DeleteAsync(releaseId, HttpContext.User, HttpContext.RequestAborted);

            return NoContent();
        }

        [HttpPost("{releaseId}/songs")]
        [Authorize(Roles = $"{UserRoles.creator},{UserRoles.admin}")]
        public async Task<IActionResult> AddSongAsync([FromRoute] string releaseId, [FromBody] AddSongToReleaseRequest request)
        {
            var song = await _releaseService.AddSongToReleaseAsync(releaseId, request, HttpContext.User, HttpContext.RequestAborted);

            return Ok(song);
        }

        [HttpDelete("{releaseId}/songs/{songId}")]
        [Authorize(Roles = $"{UserRoles.creator},{UserRoles.admin}")]
        public async Task<IActionResult> RemoveSongAsync([FromRoute] string releaseId, [FromRoute] string songId)
        {
            await _releaseService.RemoveSongFromReleaseAsync(releaseId, songId, HttpContext.User, HttpContext.RequestAborted);

            return NoContent();
        }
    }
}
