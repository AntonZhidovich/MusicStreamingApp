using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.TokenService;
using System.Security.Claims;

namespace Identity.BusinessLogic.Services.Interfaces
{
    public interface ITokenService
    {
        Task<Tokens> GetTokensAsync(GetTokensRequest request, CancellationToken cancellationToken = default);
        Task<Tokens> UseRefreshTokenAsync(Tokens tokens, CancellationToken cancellationToken = default);
        Task<ClaimsIdentity> GetIdentityFromTokenAsync(string token, CancellationToken cancellationToken = default);
    }
}
