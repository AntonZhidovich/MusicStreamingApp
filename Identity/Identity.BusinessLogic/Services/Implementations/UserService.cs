using AutoMapper;
using Identity.BusinessLogic.Constants;
using Identity.BusinessLogic.Exceptions;
using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.Messages;
using Identity.BusinessLogic.Models.UserService;
using Identity.BusinessLogic.Services.Interfaces;
using Identity.BusinessLogic.Specifications;
using Identity.DataAccess.Constants;
using Identity.DataAccess.Entities;
using Identity.DataAccess.Repositories.Interfaces;

namespace Identity.BusinessLogic.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IProducerService _producerService;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IProducerService producerService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _producerService = producerService;
        }

        public async Task<UsersPageResponse> GetAllAsync(GetUsersRequest request)
        {
            var users = await _userRepository.GetAllUsersAsync(request.CurrentPage, request.PageSize);
            var allUsersCount = await _userRepository.CountAsync();
            var usersPage = GetUsersPage(users, allUsersCount, request);

            return usersPage;
        }

        public async Task<UsersPageResponse> GetFromRegionAsync(GetUsersRequest request, string region)
        {
            var specification = new UsersFromRegionSpecification(region);
            var users = await _userRepository.ApplySpecificationAsync(specification, request.CurrentPage, request.PageSize);
            var allUsersCount = await _userRepository.CountAsync(specification);
            var usersPage = GetUsersPage(users, allUsersCount, request);

            return usersPage;
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            var user = await GetDomainUserByEmailAsync(email);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> RegisterAsync(RegisterUserRequest request)
        {
            var user = _mapper.Map<User>(request);
            
            var result = await _userRepository.AddUserAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new UnprocessableEntityException(ExceptionMessages.InvalidCredentials, result.Errors);
            }

            await _userRepository.AddUserToRoleAsync(user, UserRoles.listener);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateAsync(string email, UpdateUserRequest request)
        {
            var user = await GetDomainUserByEmailAsync(email);
            
            _mapper.Map(request, user);
            
            var result = await _userRepository.UpdateUserAsync(user);

            if (!result.Succeeded)
            {
                throw new UnprocessableEntityException(ExceptionMessages.InvalidInput, result.Errors);
            }

            var updatedDto = _mapper.Map<UserDto>(user);

            await _producerService.ProduceUserUpdatedAsync(new UserUpdatedMessage { Id = updatedDto.Id, NewUserName = updatedDto.UserName });

            return updatedDto;
        }

        public async Task DeleteAsync(DeleteUserRequest request)
        {
            var user = await GetDomainUserByEmailAsync(request.Email);
            
            await _userRepository.DeleteUserAsync(user);

            await _producerService.ProduceUserDeletedAsync(new UserDeletedMessage { Id = user.Id });
        }

        public async Task<IEnumerable<string>> GetRolesAsync(GetUserRolesRequest request)
        {
            var user = await GetDomainUserByEmailAsync(request.Email);

            return await _userRepository.GetUserRolesAsync(user);
        }

        public async Task<bool> CheckPasswordAsync(CheckPasswordRequest request)
        {
            var user = await GetDomainUserByEmailAsync(request.Email);

            return await _userRepository.CheckPasswordAsync(user, request.Password);
        }

        public async Task AddToRoleAsync(string email, RoleDto roleDto)
        {
            var user = await GetDomainUserByEmailAsync(email);
            
            await _userRepository.AddUserToRoleAsync(user, roleDto.Name);
        }

        public async Task RemoveFromRoleAsync(RemoveUserFromRoleRequest request)
        {
            var user = await GetDomainUserByEmailAsync(request.Email);
            
            await _userRepository.RemoveUserFromRoleAsync(user, request.RoleName);
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

        private UsersPageResponse GetUsersPage(IEnumerable<User> users, int allUsersCount, GetUsersRequest request)
        {
            var pagesCount = (int)Math.Ceiling((double)allUsersCount / request.PageSize);
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
