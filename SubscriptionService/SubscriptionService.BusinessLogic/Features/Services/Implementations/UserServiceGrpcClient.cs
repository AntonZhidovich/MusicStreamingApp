using SubscriptionService.BusinessLogic.Exceptions;
using SubscriptionService.BusinessLogic.Features.Services.Interfaces;
using SubscriptionService.Contracts.GrpcClients;
using System.Text;

namespace SubscriptionService.BusinessLogic.Features.Services.Implementations
{
    public class UserServiceGrpcClient : IUserServiceGrpcClient
    {
        private readonly UserService.UserServiceClient _userServiceClient;

        public UserServiceGrpcClient(UserService.UserServiceClient userServiceClient)
        {
            _userServiceClient = userServiceClient;
        }

        public async Task<bool> UserWithSuchIdExistsAsync(string id, CancellationToken cancellationToken = default)
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

        public async Task<UserInfo> GetUserInfoAsync(string id, CancellationToken cancellationToken = default)
        {
            var request = new GetUserByIdRequest { Id = id };

            var response = await _userServiceClient.GetUserByIdAsync(request, cancellationToken: cancellationToken);

            if (response.User == null)
            {
                throw new NotFoundException(response.Error);
            }

            return response.User;
        }
    }
}
