using Identity.BusinessLogic.Models.TokenService;
using Identity.BusinessLogic.Models.UserService;

namespace Identity.BusinessLogic.Services
{
    public interface ISignInService
    {
        Task<TokenResponse> SignInAsync(CheckPasswordRequest request);
    }
}
