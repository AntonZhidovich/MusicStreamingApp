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
            var authors = await _authorService.GetAllAsync(request, HttpContext.RequestAborted);

            return Ok(authors);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetAsync([FromRoute] string name)
        {
            var author = await _authorService.GetByNameAsync(name, HttpContext.RequestAborted);

            return Ok(author);
        }

        [HttpDelete("{name}")]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string name)
        {
            await _authorService.DeleteAsync(name, HttpContext.User, HttpContext.RequestAborted);

            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAuthorRequest request)
        {
            var author = await _authorService.CreateAsync(request, HttpContext.RequestAborted);

            return Ok(author);
        }

        [HttpDelete("{authorName}/artists/{userName}")]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> RemoveUserFromAuthorAsync([FromRoute] string authorName, [FromRoute] string userName)
        {
            var request = new AuthorUserRequest { AuthorName = authorName, UserName = userName };
            
            await _authorService.RemoveUserFromAuthorAsync(request , HttpContext.User, HttpContext.RequestAborted);

            return NoContent();
        }

        [HttpPost("{authorName}/artists/{userName}")]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> AddUserToAuthorAsync([FromRoute] string authorName, [FromRoute] string userName)
        {
            var request = new AuthorUserRequest { AuthorName = authorName, UserName = userName };
            
            await _authorService.AddUserToAuthorAsync(request, HttpContext.User, HttpContext.RequestAborted);

            return NoContent();
        }

        [HttpPut("{authorName}")]
        [Authorize(Roles = $"{UserRoles.admin},{UserRoles.creator}")]
        public async Task<IActionResult> UpdateAuthorAsync([FromRoute] string authorName, [FromBody] UpdateAuthorRequest request)
        {
            var author = await _authorService.UpdateAsync(authorName, request, HttpContext.User, HttpContext.RequestAborted);

            return Ok(author);
        }
    }
}
