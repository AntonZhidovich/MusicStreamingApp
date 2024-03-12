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
using Microsoft.Extensions.Logging;

namespace Identity.BusinessLogic.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IProducerService _producerService;
        private readonly IMusicUserGrpcServiceClient _musicUserServiceClient;

        public UserService(
            ILogger<UserService> logger,
            IUserRepository userRepository,
            IMapper mapper,
            IProducerService producerService,
            IMusicUserGrpcServiceClient musicUserServiceClient)
        {
            _logger = logger;
            _userRepository = userRepository;
            _mapper = mapper;
            _producerService = producerService;
            _musicUserServiceClient = musicUserServiceClient;
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

        public async Task<IEnumerable<UserDto>> GetByIdAsync(IEnumerable<string> ids)
        {
            return _mapper.Map<IEnumerable<UserDto>>(await _userRepository.GetByIdAsync(ids));
        }

        public async Task<UserDto> GetByIdAsync(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                throw new NotFoundException(ExceptionMessages.UserNotFound);
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> RegisterAsync(RegisterUserRequest request)
        {
            var user = _mapper.Map<User>(request);

            _logger.LogInformation("Attempt to registrate user {Email}", request.Email);
            
            var result = await _userRepository.AddUserAsync(user, request.Password);

            if (!result.Succeeded)
            {
                _logger.LogError("User {Email} failed to registrate. Errors: {@Errors}.", request.Email, result.Errors);

                throw new UnprocessableEntityException(ExceptionMessages.InvalidCredentials, result.Errors);
            }

            await _userRepository.AddUserToRoleAsync(user, UserRoles.listener);
            
            _logger.LogInformation("User {Email} is persisted.", request.Email);

            await _musicUserServiceClient.AddUserAsync(user);

            _logger.LogInformation("User {Email} is sent to MusicService.", request.Email);

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

            _logger.LogInformation("User {Email} is updated.", user.Email);

            return updatedDto;
        }

        public async Task DeleteAsync(string email)
        {
            var user = await GetDomainUserByEmailAsync(email);
            
            await _userRepository.DeleteUserAsync(user);

            _logger.LogInformation("User {Email} is deleted.", email);

            await _producerService.ProduceUserDeletedAsync(new UserDeletedMessage { Id = user.Id });
        }

        public async Task<bool> UserWithIdExists(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            return user != null;
        }

        private async Task<User> GetDomainUserByEmailAsync(string email)
        {
            string normalizedEmail = email.Trim().ToUpper();

            var user = await _userRepository.GetUserByEmail(normalizedEmail);

            if (user == null)
            {
                _logger.LogError("User {Email} was not found.", email);

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
