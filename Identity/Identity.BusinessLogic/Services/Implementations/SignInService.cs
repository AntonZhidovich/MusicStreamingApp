using AutoMapper;
using Identity.BusinessLogic.Constants;
using Identity.BusinessLogic.Exceptions;
using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.TokenService;
using Identity.BusinessLogic.Models.UserService;
using Identity.BusinessLogic.Services.Interfaces;

namespace Identity.BusinessLogic.Services.Implementations
{
    public class SignInService : ISignInService
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public SignInService(
            IUserService userService,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userService = userService;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<Tokens> SignInAsync(CheckPasswordRequest request, CancellationToken cancellationToken = default)
        {
            bool isAuthenticated = await _userService.CheckPasswordAsync(request);

            if (!isAuthenticated)
            {
                throw new InvalidAuthorizationException(ExceptionMessages.InvalidPassword);
            }

            var user = await _userService.GetByEmailAsync(request.Email);
            
            var roles = await _userService.GetRolesAsync(new GetUserRolesRequest { Email = user.Email });
            
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
