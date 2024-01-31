using Identity.BusinessLogic.Models;

namespace Identity.BusinessLogic.Services
{
    public interface IUserService
    {
        Task<bool> CheckPasswordAsync(CheckPasswordRequest request);
        Task<UserDto> GetByEmailAsync(GetUserByEmailRequest request);
        Task<UsersPageResponse> GetAllAsync(GetAllUsersRequest request);
        Task RegisterAsync(RegisterUserRequest request);
        Task DeleteAsync(DeleteUserRequest request);
        Task<IEnumerable<string>> GetRolesAsync(GetUserRolesRequest request);
        Task AddToRoleAsync(AddUserToRoleRequest request);
        Task RemoveFromRoleAsync(RemoveUserFromRoleAsync request);
    }
}
