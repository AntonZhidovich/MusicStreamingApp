using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Services.Interfaces;
using Identity.DataAccess.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    [Authorize(Roles = UserRoles.admin)]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            var roles = await _roleService.GetAllAsync();

            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleAsync([FromBody] RoleDto roleDto)
        {
            var role = await _roleService.AddAsync(roleDto);

            return Ok(role);
        }

        [HttpDelete("{roleName}")]
        public async Task<IActionResult> RemoveRoleAsync([FromRoute] string roleName)
        {
            await _roleService.RemoveAsync(new RoleDto { Name = roleName });

            return NoContent();
        }
    }
}
