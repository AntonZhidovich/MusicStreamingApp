using Identity.BusinessLogic.Exceptions;
using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.TokenService;
using Identity.BusinessLogic.Options;
using Identity.DataAccess.Entities;
using Identity.DataAccess.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Identity.BusinessLogic.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly ITokenRepository _tokenRepository;

        public TokenService(
            IOptions<JwtOptions> jwtOptions,
            ITokenRepository tokenRepository)
        {
            _jwtOptions = jwtOptions.Value;
            _tokenRepository = tokenRepository;
        }

        public async Task<Tokens> GetTokensAsync(GetTokensRequest request)
        {
            var identity = GetClaimsIdentity(request);
            var tokens = new Tokens
            {
                AccessToken = GenerateAccessToken(identity),
                RefreshToken = await CreateRefreshTokenAsync(request.Id)
            };

            return tokens;
        }

        public async Task<Tokens> UseRefreshTokenAsync(Tokens tokens)
        {
            await ValidateRefreshToken(tokens.RefreshToken);
            var identity = await GetIdentityFromTokenAsync(tokens.AccessToken);
            var userId = identity.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .FirstOrDefault()!;

            var newTokens = new Tokens
            {
                AccessToken = GenerateAccessToken(identity),
                RefreshToken = await CreateRefreshTokenAsync(userId)
            };

            return newTokens;
        }

        private string GenerateAccessToken(ClaimsIdentity identity)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Issuer = _jwtOptions.Issuer,
                Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_jwtOptions.AccessExpiresMins)),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(jwtToken);
        }

        private async Task<string> CreateRefreshTokenAsync(string userId)
        {
            using var generator = RandomNumberGenerator.Create();
            var number = new byte[256];
            generator.GetBytes(number);
            var tokenString = Convert.ToBase64String(number);
            var token = new RefreshToken
            {
                Token = tokenString,
                UserId = userId,
                CreatedAd = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(_jwtOptions.RefreshExpiresHours),
            };

            await _tokenRepository.DeleteTokenByUserIdAsync(token.UserId);
            await _tokenRepository.AddTokenAsync(token);
            return tokenString;
        }

        private ClaimsIdentity GetClaimsIdentity(GetTokensRequest request)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, request.Email),
                new Claim(ClaimTypes.GivenName, $"{request.FirstName} {request.LastName}"),
                new Claim(ClaimTypes.Name, request.UserName),
                new Claim(ClaimTypes.NameIdentifier, request.Id)
            };

            foreach (var role in request.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return new ClaimsIdentity(claims);
        }

        private async Task<bool> ValidateRefreshToken(string refreshToken)
        {
            var token = await _tokenRepository.GetByTokenString(refreshToken);
            if (DateTime.UtcNow > token.ExpiresAt)
            {
                throw new InvalidAuthorizationException("Refresh token expired.");
            }

            return true;
        }

        public async Task<ClaimsIdentity> GetIdentityFromTokenAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                IssuerSigningKey = key
            };

            var validationResult = await handler.ValidateTokenAsync(token, parameters);
            if (!validationResult.IsValid)
            {
                throw new InvalidAuthorizationException("Invalid access token.");
            }

            return validationResult.ClaimsIdentity;
        }
    }
}
