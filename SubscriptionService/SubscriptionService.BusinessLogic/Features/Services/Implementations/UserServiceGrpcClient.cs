using Identity.Grpc;
using Microsoft.Extensions.Logging;
using SubscriptionService.BusinessLogic.Constants;
using SubscriptionService.BusinessLogic.Exceptions;
using SubscriptionService.BusinessLogic.Features.Services.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Services.Implementations
{
    public class UserServiceGrpcClient : IUserServiceGrpcClient
    {
        private readonly UserService.UserServiceClient _userServiceClient;
        private readonly ILogger<UserServiceGrpcClient> _logger;

        public UserServiceGrpcClient(UserService.UserServiceClient userServiceClient, ILogger<UserServiceGrpcClient> logger)
        {
            _userServiceClient = userServiceClient;
            _logger = logger;
        }

        public async Task<bool> UserWithIdExistsAsync(string id, CancellationToken cancellationToken = default)
        {
            var response = await _userServiceClient.UserWithIdExistsAsync(
                new UserIdRequest { Id = id },
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
            var request = new UserIdRequest { Id = id };

            var response = await _userServiceClient.GetUserInfoAsync(request, cancellationToken: cancellationToken);

            if (response.UserInfo == null)
            {
                _logger.LogError("Error in fetching info for user {Id}", id);

                throw new NotFoundException(ExceptionMessages.userNotFound);
            }

            return response.UserInfo;
        }
    }
}
