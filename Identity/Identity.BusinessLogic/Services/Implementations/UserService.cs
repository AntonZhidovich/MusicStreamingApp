using AutoMapper;
using Identity.BusinessLogic.Exceptions;
using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.UserService;
using Identity.BusinessLogic.Services.Interfaces;
using Identity.DataAccess.Entities;
using Identity.DataAccess.Repositories.Interfaces;

namespace Identity.BusinessLogic.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UsersPageResponse> GetAllAsync(GetUsersRequest request)
        {
            var users = await _userRepository.GetAllUsersAsync(request.CurrentPage, request.PageSize);
            var allUsersCount = await _userRepository.Count();
            var usersPage = GetUsersPage(users, allUsersCount, request);

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
            var result = await _userRepository.AddUserAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new UnprocessableEntityException("Invalid credentials.", result.Errors);
            }
        }

        public async Task UpdateAsync(string email, UpdateUserRequest request)
        {
            var user = await GetDomainUserByEmailAsync(email);
            _mapper.Map(request, user);
            var result = await _userRepository.UpdateUserAsync(user);

            if (!result.Succeeded)
            {
                throw new UnprocessableEntityException("Invalid user data input.", result.Errors);
            }
        }

        public async Task DeleteAsync(DeleteUserRequest request)
        {
            var user = await GetDomainUserByEmailAsync(request.Email);
            await _userRepository.DeleteUserAsync(user);
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

        public async Task AddToRoleAsync(AddUserToRoleRequest request)
        {
            var user = await GetDomainUserByEmailAsync(request.Email);
            await _userRepository.AddUserToRoleAsync(user, request.RoleName);
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
                throw new NotFoundException("No user with such email was found");
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
