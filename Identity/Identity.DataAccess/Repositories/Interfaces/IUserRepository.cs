using Identity.DataAccess.Entities;
using Identity.DataAccess.Specifications;
using Microsoft.AspNetCore.Identity;

namespace Identity.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync(int currentPage, int pageSize);
        Task<IEnumerable<User>> ApplySpecificationAsync(ISpecification<User> specification, int currentPage, int pageSize);
        Task<User?> GetUserByEmail(string email);
        Task<int> Count();
        Task<int> Count(ISpecification<User> specification);
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task<IEnumerable<string>> GetUserRolesAsync(User user);
        Task AddUserToRoleAsync(User user, string roleName);
        Task RemoveUserFromRoleAsync(User user, string roleName);
        Task<bool> CheckPasswordAsync(User user, string password);
    }
}
