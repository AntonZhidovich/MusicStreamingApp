using AutoMapper;
using Identity.BusinessLogic.Constants;
using Identity.BusinessLogic.Exceptions;
using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.TokenService;
using Identity.BusinessLogic.Models.UserService;
using Identity.BusinessLogic.Services.Interfaces;
using Identity.DataAccess.Repositories.Interfaces;

namespace Identity.BusinessLogic.Services.Implementations
{
    public class SignInService : ISignInService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public SignInService(
            IUserRepository userRepository,
            ITokenService tokenService,
            IRoleService roleService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _roleService = roleService;
            _mapper = mapper;
        }

        public async Task<Tokens> SignInAsync(CheckPasswordRequest request, CancellationToken cancellationToken = default)
        {
            string normalizedEmail = request.Email.Trim().ToUpper();

            var user = await _userRepository.GetUserByEmail(normalizedEmail);

            if (user == null)
            {
                throw new NotFoundException(ExceptionMessages.UserNotFound);
            }

            bool isAuthenticated = await _userRepository.CheckPasswordAsync(user, request.Password);

            if (!isAuthenticated)
            {
                throw new InvalidAuthorizationException(ExceptionMessages.InvalidPassword);
            }
            
            var roles = await _roleService.GetUserRolesByEmailAsync(normalizedEmail);
            
            var tokensRequest = _mapper.Map<GetTokensRequest>(user);
            tokensRequest.Roles = roles;
            
            var tokens = await _tokenService.GetTokensAsync(tokensRequest, cancellationToken);

            return tokens;
        }

        public async Task<Tokens> SignInWithRefreshAsync(Tokens tokens, CancellationToken cancellationToken = default)
        {
            var newTokens = await _tokenService.UseRefreshTokenAsync(tokens, cancellationToken);

            return newTokens;
        }
    }
}
