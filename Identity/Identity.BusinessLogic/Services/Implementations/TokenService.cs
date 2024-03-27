using Identity.BusinessLogic.Constants;
using Identity.BusinessLogic.Exceptions;
using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.TokenService;
using Identity.BusinessLogic.Options;
using Identity.BusinessLogic.Services.Interfaces;
using Identity.DataAccess.Entities;
using Identity.DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace Identity.BusinessLogic.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger<TokenService> _logger;
        private readonly ITokenRepository _tokenRepository;

        public TokenService(
            ILogger<TokenService> logger,
            IOptions<JwtOptions> jwtOptions,
            ITokenRepository tokenRepository)
        {
            _jwtOptions = jwtOptions.Value;
            _logger = logger;
            _tokenRepository = tokenRepository;
        }

        public async Task<Tokens> GetTokensAsync(GetTokensRequest request, CancellationToken cancellationToken = default)
        {
            var identity = GetClaimsIdentity(request);
            var tokens = new Tokens
            {
                AccessToken = GenerateAccessToken(identity),
                
                RefreshToken = await CreateRefreshTokenAsync(request.Id, cancellationToken)
            };

            return tokens;
        }

        public async Task<Tokens> UseRefreshTokenAsync(Tokens tokens, CancellationToken cancellationToken = default)
        {
            await ValidateRefreshTokenAsync(tokens.RefreshToken, cancellationToken);
            
            var identity = await GetIdentityFromTokenAsync(tokens.AccessToken, cancellationToken);
            
            var userId = identity.Claims
                .Where(claim => claim.Type == ClaimTypes.NameIdentifier)
                .Select(claim => claim.Value)
                .FirstOrDefault()!;

            var newTokens = new Tokens
            {
                AccessToken = GenerateAccessToken(identity),
                
                RefreshToken = await CreateRefreshTokenAsync(userId, cancellationToken)
            };

            return newTokens;
        }

        public async Task<ClaimsIdentity> GetIdentityFromTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            var handler = new JwtSecurityTokenHandler();
            var parameters = GetTokenValidationParameters();
    
            var validationResult = await handler.ValidateTokenAsync(token, parameters);

            if (!validationResult.IsValid)
            {
                throw new InvalidAuthorizationException(ExceptionMessages.InvalidAccessToken);
            }

            return validationResult.ClaimsIdentity;
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
            _logger.LogInformation("Access token for {userName} is generated.", identity.Name);

            return tokenHandler.WriteToken(jwtToken);
        }

        private async Task<string> CreateRefreshTokenAsync(string userId, CancellationToken cancellationToken = default)
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

            var oldToken = await _tokenRepository.GetTokenByUserIdAsync(userId, cancellationToken);

            if (oldToken != null)
            {
                await _tokenRepository.DeleteTokenAsync(oldToken, cancellationToken);
            }
            
            await _tokenRepository.AddTokenAsync(token, cancellationToken);
            _logger.LogInformation("Refresh token for user with id {userId} is generated.", userId);

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

            claims.AddRange(request.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

            return new ClaimsIdentity(claims);
        }

        private async Task<bool> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            var token = await _tokenRepository.GetByTokenString(refreshToken, cancellationToken);

            if (token == null)
            {
                throw new NotFoundException(ExceptionMessages.InvalidRefreshToken);
            }

            if (DateTime.UtcNow > token.ExpiresAt)
            {
                throw new InvalidAuthorizationException(ExceptionMessages.TokenExpired);
            }

            return true;
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

            return new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                IssuerSigningKey = key
            };
        }
    }
}
