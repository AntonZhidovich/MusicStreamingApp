using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllRolesAsync(CancellationToken cancellationToken)
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddRoleAsync([FromBody] RoleDto role, CancellationToken cancellationToken)
        {
            await _roleService.AddAsync(role);
            return NoContent();
        }

        [HttpDelete("{roleName}")]
        public async Task<IActionResult> RemoveRoleAsync([FromRoute] string roleName, CancellationToken cancellationToken)
        {
            await _roleService.RemoveAsync(new RoleDto { RoleName = roleName });
            return NoContent();
        }
    }
}
