using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.UserService;

namespace Identity.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<UsersPageResponse> GetAllAsync(GetUsersRequest request);
        Task<UsersPageResponse> GetFromRegionAsync(GetUsersRequest request, string region);
        Task<UserDto> GetByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(CheckPasswordRequest request);
        Task<UserDto> RegisterAsync(RegisterUserRequest request);
        Task<UserDto> UpdateAsync(string email, UpdateUserRequest request);
        Task DeleteAsync(DeleteUserRequest request);
        Task<IEnumerable<string>> GetRolesAsync(GetUserRolesRequest request);
        Task AddToRoleAsync(string email, RoleDto roleDto);
        Task RemoveFromRoleAsync(RemoveUserFromRoleRequest request);
    }
}
