using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicService.Application.Interfaces;
using MusicService.Application.Models.AuthorService;
using MusicService.Domain.Constants;

namespace MusicService.API.Controllers
{
    [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
    [Route("api/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync() 
        {
            var authors = await _authorService.GetAllAsync();

            return Ok(authors);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetAllAsync([FromRoute] string name)
        {
            var author = await _authorService.GetByNameAsync(name);

            return Ok(author);
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string name)
        {
            await _authorService.DeleteAsync(name);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAuthorRequest request)
        {
            await _authorService.CreateAsync(request);

            return NoContent();
        }

        [HttpDelete("{authorName}/artists/{artistName}")]
        public async Task<IActionResult> RemoveArtistFromAuthorAsync([FromRoute] string authorName, [FromRoute] string artistName)
        {
            var request = new AuthorArtistRequest { AuthorName = authorName, ArtistUserName = artistName };
            await _authorService.RemoveArtistFromAuthorAsync(request);

            return NoContent();
        }

        [HttpPost("{authorName}/artists/{artistName}")]
        public async Task<IActionResult> AddArtistToAuthorAsync([FromRoute] string authorName, [FromRoute] string artistName)
        {
            var request = new AuthorArtistRequest { AuthorName = authorName, ArtistUserName = artistName };
            await _authorService.AddArtistToAuthorAsync(request);

            return NoContent();
        }

        [HttpPatch("{authorName}/description")]
        public async Task<IActionResult> UpdateAuthorDescriptionAsync([FromRoute] string authorName, [FromBody] UpdateAuthorDescriptionRequest request)
        {
            await _authorService.UpdateDesctiptionAsync(authorName, request);

            return NoContent();
        }

        [HttpPost("{authorName}/broken-at")]
        public async Task<IActionResult> BreakArtistAsync([FromRoute] string authorName, [FromBody] BreakAuthorRequest request)
        {
            await _authorService.BreakAuthorAsync(authorName, request);

            return NoContent();
        }

        [HttpDelete("{authorName}/broken-at")]
        public async Task<IActionResult> BreakArtistAsync([FromRoute] string authorName)
        {
            await _authorService.UnbreakAuthorAsync(authorName);

            return NoContent();
        }
    }
}
