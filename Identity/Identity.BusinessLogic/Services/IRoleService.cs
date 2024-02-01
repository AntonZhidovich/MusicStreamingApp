using Identity.BusinessLogic.Models;

namespace Identity.BusinessLogic.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllAsync();
        Task AddAsync(RoleDto role);
        Task RemoveAsync(RoleDto role);
    }
}
