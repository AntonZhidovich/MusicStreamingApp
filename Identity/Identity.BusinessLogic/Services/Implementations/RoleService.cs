using AutoMapper;
using Identity.BusinessLogic.Constants;
using Identity.BusinessLogic.Exceptions;
using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.BusinessLogic.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<RoleDto> AddAsync(RoleDto roleDto)
        {
            var role = new IdentityRole(roleDto.Name);

            await _roleManager.CreateAsync(role);

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
        }
    }
}
