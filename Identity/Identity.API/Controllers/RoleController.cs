using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RoleDto>))]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AddRoleAsync([FromBody] RoleDto role)
        {
            await _roleService.AddAsync(role);
            return Ok();
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        public async Task<IActionResult> RemoveRoleAsync([FromBody] RoleDto role)
        {
            await _roleService.RemoveAsync(role);
            return Ok();
        }
    }
}
