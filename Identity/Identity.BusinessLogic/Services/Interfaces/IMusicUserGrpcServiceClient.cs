using Identity.BusinessLogic.GrpcClients;
using Identity.DataAccess.Entities;

namespace Identity.BusinessLogic.Services.Interfaces
{
    public interface IMusicUserGrpcServiceClient
    {
        Task<AddUserResponse> AddUserAsync(User user, IEnumerable<string> roles, CancellationToken cancellationToken = default);
    }
}
