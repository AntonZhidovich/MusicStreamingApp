using AutoMapper;
using Identity.BusinessLogic.Constants;
using Identity.BusinessLogic.Exceptions;
using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.TokenService;
using Identity.BusinessLogic.Models.UserService;
using Identity.BusinessLogic.Services.Interfaces;
using Identity.DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Identity.BusinessLogic.Services.Implementations
{
    public class SignInService : ISignInService
    {
        private readonly ILogger<SignInService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public SignInService(
            ILogger<SignInService> logger,
            IUserRepository userRepository,
            ITokenService tokenService,
            IRoleService roleService,
            IMapper mapper)
        {
            _logger = logger;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _roleService = roleService;
            _mapper = mapper;
        }

        public async Task<Tokens> SignInAsync(CheckPasswordRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("User {Email} attemts to sign in.", request.Email);

            string normalizedEmail = request.Email.Trim().ToUpper();

            var user = await _userRepository.GetUserByEmail(normalizedEmail);

            if (user == null)
            {
                _logger.LogError("User with email {Email} was not found.", request.Email);

                throw new NotFoundException(ExceptionMessages.UserNotFound);
            }

            bool isAuthenticated = await _userRepository.CheckPasswordAsync(user, request.Password);

            if (!isAuthenticated)
            {
                _logger.LogError("User with email {Email} entered wrong password.", request.Email);

                throw new InvalidAuthorizationException(ExceptionMessages.InvalidPassword);
            }
            
            var roles = await _roleService.GetUserRolesByEmailAsync(normalizedEmail);
            
            var tokensRequest = _mapper.Map<GetTokensRequest>(user);
            tokensRequest.Roles = roles;
            
            var tokens = await _tokenService.GetTokensAsync(tokensRequest, cancellationToken);

            _logger.LogInformation("User {Email} signed in with credentials.", request.Email);

            return tokens;
        }

        public async Task<Tokens> SignInWithRefreshAsync(Tokens tokens, CancellationToken cancellationToken = default)
        {
            var newTokens = await _tokenService.UseRefreshTokenAsync(tokens, cancellationToken);

            _logger.LogInformation("User signed in with refresh token");

            return newTokens;
        }
    }
}
