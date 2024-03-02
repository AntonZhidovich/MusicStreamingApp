using Identity.Grpc;

namespace SubscriptionService.BusinessLogic.Features.Services.Interfaces
{
    public interface IUserServiceGrpcClient
    {
        Task<bool> UserWithIdExistsAsync(string id, CancellationToken cancellationToken = default);
        Task<GetIdUserNameMapResponse> GetIdUserNameMap(IEnumerable<string> ids, CancellationToken cancellationToken = default);
    }
}
