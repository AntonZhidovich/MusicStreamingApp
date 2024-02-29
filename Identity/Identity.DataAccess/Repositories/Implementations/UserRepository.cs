using Identity.DataAccess.Entities;
using Identity.DataAccess.Extensions;
using Identity.DataAccess.Repositories.Interfaces;
using Identity.DataAccess.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.DataAccess.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(int currentPage, int pageSize)
        {
            return await _userManager.Users.GetPage(currentPage, pageSize).ToListAsync();
        }

        public async Task<IEnumerable<User>> ApplySpecificationAsync(ISpecification<User> specification, int currentPage, int pageSize)
        {
            var users = await _userManager.Users
                .ApplySpecification(specification)
                .GetPage(currentPage, pageSize)
                .ToListAsync();

            return users;
        }

        public Task<User?> GetUserByEmail(string email)
        {
            return _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<User>> GetByIdAsync(IEnumerable<string> ids)
        {
            return await _userManager.Users
                .Where(user => ids.Contains(user.Id))
                .ToListAsync();
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<int> CountAsync()
        {
            return await _userManager.Users.CountAsync();
        }

        public async Task<int> CountAsync(ISpecification<User> specification)
        {
            return await _userManager.Users.ApplySpecification(specification).CountAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            var result = await _userManager.UpdateAsync(user);

            return result;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task RemoveUserFromRoleAsync(User user, string roleName)
        {
            await _userManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}
