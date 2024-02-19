using Identity.BusinessLogic.Models;

namespace Identity.BusinessLogic.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllAsync();
        Task<RoleDto> AddAsync(RoleDto roleDto);
        Task RemoveAsync(RoleDto role);
    }
}
