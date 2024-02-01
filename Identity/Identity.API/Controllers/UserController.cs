using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.UserService;
using Identity.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(UsersPageResponse))]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] GetUsersRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _userService.GetAllAsync(request));
        }

        [HttpGet("{email}")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        public async Task<IActionResult> GetUserByEmailAsync([FromRoute] string email, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByEmailAsync(new GetUserByEmailRequest { Email = email });
            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            await _userService.RegisterAsync(request);
            return Ok();
        }

        [HttpDelete("{email}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] string email, CancellationToken cancellationToken)
        {
            await _userService.DeleteAsync(new DeleteUserRequest { Email = email });
            return Ok();
        }

        [HttpGet("Roles/{email}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<string>))]
        public async Task<IActionResult> GetUserRolesAsync([FromRoute] string email, CancellationToken cancellationToken)
        {
            var roles = await _userService.GetRolesAsync(new GetUserRolesRequest { Email = email });
            return Ok(roles);
        }

        [HttpPost("Roles")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AddUserToRoleAsync(AddUserToRoleRequest request, CancellationToken cancellationToken)
        {
            await _userService.AddToRoleAsync(request);
            return Ok();
        }

        [HttpDelete("Roles")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> RemoveUserFromRoleAsync(RemoveUserFromRoleAsync request, CancellationToken cancellationToken)
        {
            await _userService.RemoveFromRoleAsync(request);
            return Ok();
        }
    }
}
