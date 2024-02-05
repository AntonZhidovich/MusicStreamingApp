using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.UserService;
using Identity.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] GetUsersRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _userService.GetAllAsync(request));
        }

        [HttpGet("from-region/{region}")]
        public async Task<IActionResult> GetUsersFromRegionAsync([FromRoute] string region, [FromQuery] GetUsersRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _userService.GetFromRegionAsync(request, region));
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserByEmailAsync([FromRoute] string email, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByEmailAsync(new GetUserByEmailRequest { Email = email });

            return Ok(user);
        }

        [HttpPost("")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            await _userService.RegisterAsync(request);

            return NoContent();
        }

        [HttpPut("{email}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string email, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
        {
            await _userService.UpdateAsync(email, request);

            return NoContent();
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] string email, CancellationToken cancellationToken)
        {
            await _userService.DeleteAsync(new DeleteUserRequest { Email = email });

            return NoContent();
        }

        [HttpGet("{email}/roles")]
        public async Task<IActionResult> GetUserRolesAsync([FromRoute] string email, CancellationToken cancellationToken)
        {
            var roles = await _userService.GetRolesAsync(new GetUserRolesRequest { Email = email });

            return Ok(roles);
        }

        [HttpPost("{email}/roles")]
        public async Task<IActionResult> AddUserToRoleAsync([FromRoute] string email, [FromBody] RoleDto roleDto, CancellationToken cancellationToken)
        {
            await _userService.AddToRoleAsync(email, roleDto);

            return NoContent();
        }

        [HttpDelete("{email}/roles/{rolename}")]
        public async Task<IActionResult> RemoveUserFromRoleAsync([FromRoute] string email, [FromRoute] string rolename)
        {
            await _userService.RemoveFromRoleAsync(new RemoveUserFromRoleRequest { Email = email, RoleName = rolename});

            return NoContent();
        }
    }
}
