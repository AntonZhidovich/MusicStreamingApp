using AutoMapper;
using Identity.BusinessLogic.Exceptions;
using Identity.BusinessLogic.Models;
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

        public async Task<IEnumerable<GetUserDto>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersDto = _mapper.Map<IEnumerable<GetUserDto>>(users);
            return usersDto;
        }

        public async Task<GetUserDto> GetByEmailAsync(GetUserByEmailRequest request)
        {
            var user = await GetDomainUserByEmailAsync(request.Email);
            return _mapper.Map<GetUserDto>(user);
        }

        public async Task RegisterAsync(RegisterUserRequest request)
        {
            var user = _mapper.Map<User>(request);
            var result = await _userManager.CreateAsync(user, request.Password);
            if(!result.Succeeded)
            {
                throw new InvalidAuthorizationException("Invalid authorization input.", result.Errors);
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
    }
}
