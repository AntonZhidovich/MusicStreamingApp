using Identity.BusinessLogic.Models.TokenService;
using Identity.BusinessLogic.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.BusinessLogic.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _jwtOptions;

        public TokenService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public TokenResponse GetTokens(GetTokensRequest request)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, request.Email),
                new Claim(ClaimTypes.GivenName, $"{request.FirstName} {request.LastName}"),
                new Claim(ClaimTypes.Name, request.UserName)
            };

            foreach(var role in request.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtOptions.Issuer,
                Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_jwtOptions.ExpirationMins)),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.CreateToken(descriptor);

            return new TokenResponse
            {
                Token = tokenHandler.WriteToken(jwtToken),
                RefreshToken = "refresh token"
            };
        }
    }
}
