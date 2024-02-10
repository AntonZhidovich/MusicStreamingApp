using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicService.Application.Interfaces;
using MusicService.Application.Models;
using MusicService.Application.Models.AuthorService;
using MusicService.Domain.Constants;

namespace MusicService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/authors")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetPageRequest request) 
        {
            var authors = await _authorService.GetAllAsync(request);

            return Ok(authors);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetAsync([FromRoute] string name)
        {
            var author = await _authorService.GetByNameAsync(name);

            return Ok(author);
        }

        [HttpDelete("{name}")]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string name)
        {
            await _authorService.DeleteAsync(name, HttpContext.User);

            return NoContent();
        }


        [HttpPost]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAuthorRequest request)
        {
            await _authorService.CreateAsync(request);

            return NoContent();
        }

        [HttpDelete("{authorName}/artists/{artistName}")]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> RemoveArtistFromAuthorAsync([FromRoute] string authorName, [FromRoute] string artistName)
        {
            var request = new AuthorArtistRequest { AuthorName = authorName, ArtistUserName = artistName };
            await _authorService.RemoveArtistFromAuthorAsync(request , HttpContext.User);

            return NoContent();
        }

        [HttpPost("{authorName}/artists/{artistName}")]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> AddArtistToAuthorAsync([FromRoute] string authorName, [FromRoute] string artistName)
        {
            var request = new AuthorArtistRequest { AuthorName = authorName, ArtistUserName = artistName };
            await _authorService.AddArtistToAuthorAsync(request, HttpContext.User);

            return NoContent();
        }

        [HttpPut("{authorName}/description")]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> UpdateAuthorAsync([FromRoute] string authorName, [FromBody] UpdateAuthorRequest request)
        {
            await _authorService.UpdateAsync(authorName, request, HttpContext.User);

            return NoContent();
        }
    }
}
