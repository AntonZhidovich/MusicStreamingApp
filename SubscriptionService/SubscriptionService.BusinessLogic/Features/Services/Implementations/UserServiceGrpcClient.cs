using Identity.Grpc;
using SubscriptionService.BusinessLogic.Features.Services.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Services.Implementations
{
    public class UserServiceGrpcClient : IUserServiceGrpcClient
    {
        private readonly UserService.UserServiceClient _userServiceClient;

        public UserServiceGrpcClient(UserService.UserServiceClient userServiceClient)
        {
            _userServiceClient = userServiceClient;
        }

        public async Task<bool> UserWithIdExistsAsync(string id, CancellationToken cancellationToken = default)
        {
            var response = await _userServiceClient.UserWithIdExistsAsync(
                new UserWithIdExistsRequest { Id = id },
                cancellationToken: cancellationToken);

            return response.UserExists;
        }

        public async Task<GetIdUserNameMapResponse> GetIdUserNameMap(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            var request = new GetIdUserNameMapRequest();
            request.Ids.AddRange(ids);

            var response = await _userServiceClient.GetIdUserNameMapAsync(request, cancellationToken: cancellationToken);

            return response;
        }
    }
}
