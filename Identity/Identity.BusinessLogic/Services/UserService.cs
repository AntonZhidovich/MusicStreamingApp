using AutoMapper;
using Identity.BusinessLogic.Exceptions;
using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.UserService;
using Identity.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(
            UserManager<User> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UsersPageResponse> GetAllAsync(GetUsersRequest request)
        {
            var users = await _userManager.Users
                .Skip(request.PageSize * (request.CurrentPage - 1))
                .Take(request.PageSize)
                .OrderByDescending(u => u.UserName)
                .ToListAsync();
            var usersPage = GetUsersPage(users, request);

            return usersPage;
        }

        public async Task<UserDto> GetByEmailAsync(GetUserByEmailRequest request)
        {
            var user = await GetDomainUserByEmailAsync(request.Email);
            return _mapper.Map<UserDto>(user);
        }

        public async Task RegisterAsync(RegisterUserRequest request)
        {
            var user = _mapper.Map<User>(request);
            var result = await _userManager.CreateAsync(user, request.Password);
            if(!result.Succeeded)
            {
                throw new InvalidAuthorizationException("Invalid credentials.", result.Errors);
            }
        }

        public async Task DeleteAsync(DeleteUserRequest request)
        {
            var user =  await GetDomainUserByEmailAsync(request.Email);
            await _userManager.DeleteAsync(user);
        }

        public async Task<IEnumerable<string>> GetRolesAsync(GetUserRolesRequest request)
        {
            var user = await GetDomainUserByEmailAsync(request.Email);
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> CheckPasswordAsync(CheckPasswordRequest request)
        {
            var user = await GetDomainUserByEmailAsync(request.Email);
            return await _userManager.CheckPasswordAsync(user, request.Password);
        }

        public async Task AddToRoleAsync(AddUserToRoleRequest request)
        {
            var user = await GetDomainUserByEmailAsync(request.Email);
            await _userManager.AddToRoleAsync(user, request.RoleName);
        }

        public async Task RemoveFromRoleAsync(RemoveUserFromRoleAsync request)
        {
            var user = await GetDomainUserByEmailAsync(request.Email);
            await _userManager.RemoveFromRoleAsync(user, request.RoleName);
        }

        private async Task<User> GetDomainUserByEmailAsync(string email)
        {
            string normalizedEmail = email.Trim().ToUpper();
            var user = await _userManager.FindByEmailAsync(normalizedEmail);
            if (user == null)
            {
                throw new ArgumentException("No user with such email was found");
            }

            return user;
        }

        private UsersPageResponse GetUsersPage(IEnumerable<User> users, GetUsersRequest request)
        {
            var usersCount = users.Count();
            var pagesCount = (int)Math.Ceiling((double)usersCount / request.PageSize);
            var usersPage = new UsersPageResponse
            {
                PagesCount = pagesCount,
                CurrentPage = request.CurrentPage,
                PageSize = request.PageSize,
                users = _mapper.Map<IEnumerable<UserDto>>(users)
            };

            return usersPage;
        }
    }
}
