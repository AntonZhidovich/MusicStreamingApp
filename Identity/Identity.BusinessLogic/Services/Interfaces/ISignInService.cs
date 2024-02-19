using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.UserService;

namespace Identity.BusinessLogic.Services.Interfaces
{
    public interface ISignInService
    {
        Task<Tokens> SignInAsync(CheckPasswordRequest request, CancellationToken cancellationToken = default);
        Task<Tokens> SignInWithRefreshAsync(Tokens tokens, CancellationToken cancellationToken = default);
    }
}
