using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.UserService;
using Identity.BusinessLogic.Services.Interfaces;
using Identity.DataAccess.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.admin)]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] GetUsersRequest request)
        {
            return Ok(await _userService.GetAllAsync(request));
        }

        [HttpGet("region/{region}")]
        [Authorize(Roles = UserRoles.admin)]
        public async Task<IActionResult> GetUsersFromRegionAsync([FromRoute] string region, [FromQuery] GetUsersRequest request)
        {
            return Ok(await _userService.GetFromRegionAsync(request, region));
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserByEmailAsync([FromRoute] string email)
        {
            var user = await _userService.GetByEmailAsync(email);

            return Ok(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequest request)
        {
            var user = await _userService.RegisterAsync(request);

            return Ok(user);
        }

        [HttpPut("{email}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string email, [FromBody] UpdateUserRequest request)
        {
            var user = await _userService.UpdateAsync(email, request);

            return Ok(user);
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] string email)
        {
            await _userService.DeleteAsync(email);

            return NoContent();
        }

        [HttpGet("{email}/roles")]
        [Authorize(Roles = UserRoles.admin)]
        public async Task<IActionResult> GetUserRolesAsync([FromRoute] string email)
        {
            var roles = await _roleService.GetUserRolesByEmailAsync(email);

            return Ok(roles);
        }

        [HttpPost("{email}/roles/{roleName}")]
        [Authorize(Roles = UserRoles.admin)]
        public async Task<IActionResult> AddUserToRoleAsync([FromRoute] string email, [FromRoute] string roleName)
        {
            await _roleService.AddUserToRoleAsync(email, roleName);

            return NoContent();
        }

        [HttpDelete("{email}/roles/{roleName}")]
        [Authorize(Roles = UserRoles.admin)]
        public async Task<IActionResult> RemoveUserFromRoleAsync([FromRoute] string email, [FromRoute] string roleName)
        {
            await _roleService.RemoveUserFromRoleAsync(email, roleName);

            return NoContent();
        }
    }
}
