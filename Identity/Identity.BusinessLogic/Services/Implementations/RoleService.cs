using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.BusinessLogic.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task AddAsync(RoleDto role)
        {
            await _roleManager.CreateAsync(new IdentityRole(role.RoleName));
        }

        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            var roles = await _roleManager.Roles
                .Select(r => new RoleDto { RoleName = r.Name! })
                .ToListAsync();

            return roles;
        }

        public async Task RemoveAsync(RoleDto role)
        {
            var identityRole = await _roleManager.FindByNameAsync(role.RoleName);
            if (identityRole != null)
            {
                await _roleManager.DeleteAsync(identityRole);
            }
        }
    }
}
