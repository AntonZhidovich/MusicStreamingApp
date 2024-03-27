using AutoMapper;
using Identity.BusinessLogic.Constants;
using Identity.BusinessLogic.Exceptions;
using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Services.Interfaces;
using Identity.DataAccess.Entities;
using Identity.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Identity.BusinessLogic.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly ILogger<RoleService> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public RoleService(
            ILogger<RoleService> logger,
            RoleManager<IdentityRole> roleManager, 
            IMapper mapper, 
            IUserRepository userRepository)
        {
            _logger = logger;
            _roleManager = roleManager;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<RoleDto> AddAsync(RoleDto roleDto)
        {
            var role = new IdentityRole(roleDto.Name);

            await _roleManager.CreateAsync(role);

            _logger.LogInformation("Role {roleName} is created.", roleDto.Name);

            return _mapper.Map<RoleDto>(role);
        }

        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            var roles = await _roleManager.Roles
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<RoleDto>>(roles);

            return dtos;
        }

        public async Task RemoveAsync(RoleDto role)
        {
            var identityRole = await _roleManager.FindByNameAsync(role.Name);
            
            if (identityRole == null)
            {
                throw new NotFoundException(ExceptionMessages.RoleNotFound);
            }

            await _roleManager.DeleteAsync(identityRole);
            
            _logger.LogInformation("Role {roleName} is removed.", role.Name);
        }

        public async Task<IEnumerable<string>> GetUserRolesByEmailAsync(string email)
        {
            var user = await GetDomainUserByEmailAsync(email);

            return await _userRepository.GetUserRolesAsync(user);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException(ExceptionMessages.UserNotFound);
            }

            return await _userRepository.GetUserRolesAsync(user);
        }

        public async Task AddUserToRoleAsync(string email, string roleName)
        {
            var user = await GetDomainUserByEmailAsync(email);

            await _userRepository.AddUserToRoleAsync(user, roleName);

            _logger.LogInformation("User {Email} is added to role {roleName}", email, roleName);
        }

        public async Task RemoveUserFromRoleAsync(string email, string roleName)
        {
            var user = await GetDomainUserByEmailAsync(email);

            await _userRepository.RemoveUserFromRoleAsync(user, roleName);

            _logger.LogInformation("User {Email} is removed from role {roleName}", email, roleName);
        }

        private async Task<User> GetDomainUserByEmailAsync(string email)
        {
            string normalizedEmail = email.Trim().ToUpper();

            var user = await _userRepository.GetUserByEmail(normalizedEmail);

            if (user == null)
            {
                throw new NotFoundException(ExceptionMessages.UserNotFound);
            }

            return user;
        }
    }
}
