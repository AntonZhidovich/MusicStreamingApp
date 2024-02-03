using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.UserService;

namespace Identity.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<UsersPageResponse> GetAllAsync(GetUsersRequest request);
        Task<UsersPageResponse> GetFromRegionAsync(GetUsersRequest request, string region);
        Task<UserDto> GetByEmailAsync(GetUserByEmailRequest request);
        Task<bool> CheckPasswordAsync(CheckPasswordRequest request);
        Task RegisterAsync(RegisterUserRequest request);
        Task UpdateAsync(string email, UpdateUserRequest request);
        Task DeleteAsync(DeleteUserRequest request);
        Task<IEnumerable<string>> GetRolesAsync(GetUserRolesRequest request);
        Task AddToRoleAsync(AddUserToRoleRequest request);
        Task RemoveFromRoleAsync(RemoveUserFromRoleRequest request);
    }
}
