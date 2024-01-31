using Identity.BusinessLogic.Models.TokenService;

namespace Identity.BusinessLogic.Services
{
    public interface ITokenService
    {
        TokenResponse GetTokens(GetTokensRequest request);
    }
}
