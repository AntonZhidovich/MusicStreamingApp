using Identity.BusinessLogic.Models;

namespace Identity.BusinessLogic.Services
{
    public interface ISignInService
    {
        Task<TokenResponse> SignInAsync(CheckPasswordRequest request);
    }
}
