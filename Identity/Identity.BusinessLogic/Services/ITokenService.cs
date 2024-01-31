using Identity.BusinessLogic.Models;

namespace Identity.BusinessLogic.Services
{
    public interface ITokenService
    {
        TokenResponse GetTokens(GetTokensRequest request);
    }
}
