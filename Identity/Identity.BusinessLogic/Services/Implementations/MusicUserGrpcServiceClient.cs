using AutoMapper;
using Identity.BusinessLogic.GrpcClients;
using Identity.BusinessLogic.Services.Interfaces;
using Identity.DataAccess.Entities;

namespace Identity.BusinessLogic.Services.Implementations
{
    public class MusicUserGrpcServiceClient : IMusicUserGrpcServiceClient
    {
        private readonly MusicUserService.MusicUserServiceClient _musicUserServiceClient;
        private readonly IMapper _mapper;

        public MusicUserGrpcServiceClient(MusicUserService.MusicUserServiceClient musicUserServiceClient,
            IMapper mapper)
        {
            _musicUserServiceClient = musicUserServiceClient;
            _mapper = mapper;
        }

        public async Task<AddUserResponse> AddUserAsync(User user, 
            IEnumerable<string> roles, 
            CancellationToken cancellationToken = default)
        {
            var request = _mapper.Map<AddUserRequest>(user);
            request.Roles.AddRange(roles);

            await _musicUserServiceClient.AddUserAsync(request, 
                cancellationToken: cancellationToken);

            return new AddUserResponse();
        }
    }
}
