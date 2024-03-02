using Identity.DataAccess.Entities;
using MusicService.Grpc;

namespace Identity.BusinessLogic.Services.Interfaces
{
    public interface IMusicUserGrpcServiceClient
    {
        Task<AddUserResponse> AddUserAsync(User user, IEnumerable<string> roles, CancellationToken cancellationToken = default);
    }
}
