using Identity.Grpc;
using MusicService.Application.Interfaces;

namespace MusicService.Application.Services
{
    public class UserServiceGrpcClient : IUserServiceGrpcClient
    {
        private readonly UserService.UserServiceClient _userServiceClient;

        public UserServiceGrpcClient(UserService.UserServiceClient userServiceClient)
        {
            _userServiceClient = userServiceClient;
        }

        public async Task<bool> UserIsInRoleAsync(string id, string roleName, CancellationToken cancellationToken = default)
        {
            var request = new UserIsInRoleRequest { Id = id, RoleName = roleName };

            var response = await _userServiceClient.UserIsInRoleAsync(request, cancellationToken:  cancellationToken);

            return response.UserIsInRole;
        }
    }
}
