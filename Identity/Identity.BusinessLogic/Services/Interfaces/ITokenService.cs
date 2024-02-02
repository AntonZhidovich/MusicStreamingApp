using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.TokenService;
using System.Security.Claims;

namespace Identity.BusinessLogic.Services.Interfaces
{
    public interface ITokenService
    {
        Task<Tokens> GetTokensAsync(GetTokensRequest request);
        Task<Tokens> UseRefreshTokenAsync(Tokens tokens);
        Task<ClaimsIdentity> GetIdentityFromTokenAsync(string token);
    }
}
