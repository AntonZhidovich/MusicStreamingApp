using AutoMapper;
using Identity.BusinessLogic.Models;

namespace Identity.BusinessLogic.Services
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

        public async Task<TokenResponse> SignInAsync(CheckPasswordRequest request)
        {
            bool isAuthenticated = await _userService.CheckPasswordAsync(request);
            if (!isAuthenticated)
            {
                throw new ArgumentException("Invalid password.");
            }

            var user = await _userService.GetByEmailAsync(new GetUserByEmailRequest { Email = request.Email });
            var roles = await _userService.GetRolesAsync(new GetUserRolesRequest { Email = user.Email });
            var tokensRequest = _mapper.Map<GetTokensRequest>(user);
            tokensRequest.Roles = roles;
            var tokens = _tokenService.GetTokens(tokensRequest);

            return tokens;
        }
    }
}
