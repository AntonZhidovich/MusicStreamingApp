using Identity.BusinessLogic.Models;

namespace Identity.BusinessLogic.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllAsync();
        Task<RoleDto> AddAsync(RoleDto roleDto);
        Task RemoveAsync(RoleDto role);
        Task<IEnumerable<string>> GetUserRolesByEmailAsync(string email);
        Task<IEnumerable<string>> GetUserRolesAsync(string userId);
        Task AddUserToRoleAsync(string email, string roleName);
        Task RemoveUserFromRoleAsync(string email, string roleName);
    }
}
