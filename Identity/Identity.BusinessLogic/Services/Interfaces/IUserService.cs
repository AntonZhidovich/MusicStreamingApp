using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.UserService;

namespace Identity.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<UsersPageResponse> GetAllAsync(GetUsersRequest request);
        Task<UsersPageResponse> GetFromRegionAsync(GetUsersRequest request, string region);
        Task<UserDto> GetByEmailAsync(string email);
        Task<UserDto> GetByIdAsync(string id);
        Task<IEnumerable<UserDto>> GetByIdAsync(IEnumerable<string> ids);
        Task<bool> UserWithIdExists(string id);
        Task<UserDto> RegisterAsync(RegisterUserRequest request);
        Task<UserDto> UpdateAsync(string email, UpdateUserRequest request);
        Task DeleteAsync(string email);
    }
}
