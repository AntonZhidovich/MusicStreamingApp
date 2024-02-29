using SubscriptionService.Contracts.GrpcClients;

namespace SubscriptionService.BusinessLogic.Features.Services.Interfaces
{
    public interface IUserServiceGrpcClient
    {
        Task<bool> UserWithSuchIdExistsAsync(string id, CancellationToken cancellationToken = default);
        Task<GetIdUserNameMapResponse> GetIdUserNameMap(IEnumerable<string> ids, CancellationToken cancellationToken = default);
        Task<UserInfo> GetUserInfoAsync(string id, CancellationToken cancellationToken = default);
    }
}
