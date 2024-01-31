using Identity.BusinessLogic.Models.TokenService;
using Identity.BusinessLogic.Options;
using Identity.DataAccess.Entities;
using Microsoft.Extensions.Options;

namespace Identity.BusinessLogic.Services
{
    public class TokenService : ITokenService
    {
        private readonly IOptions<JwtOptions> _jwtOptions;

        public TokenService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }

        public TokenResponse GetTokens(GetTokensRequest request)
        {
            return new TokenResponse
            {
                Token = "access token",
                RefreshToken = "refresh token"
            };
        }
    }
}
