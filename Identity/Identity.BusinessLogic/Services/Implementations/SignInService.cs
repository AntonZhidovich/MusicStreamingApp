using AutoMapper;
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

        public async Task<Tokens> SignInAsync(CheckPasswordRequest request)
        {
            bool isAuthenticated = await _userService.CheckPasswordAsync(request);
            if (!isAuthenticated)
            {
                throw new InvalidAuthorizationException("Invalid password.");
            }

            var user = await _userService.GetByEmailAsync(new GetUserByEmailRequest { Email = request.Email });
            var roles = await _userService.GetRolesAsync(new GetUserRolesRequest { Email = user.Email });
            var tokensRequest = _mapper.Map<GetTokensRequest>(user);
            tokensRequest.Roles = roles;
            var tokens = await _tokenService.GetTokensAsync(tokensRequest);

            return tokens;
        }

        public async Task<Tokens> SignInWithRefreshAsync(Tokens tokens)
        {
            var newTokens = await _tokenService.UseRefreshTokenAsync(tokens);
            return newTokens;
        }
    }
}
