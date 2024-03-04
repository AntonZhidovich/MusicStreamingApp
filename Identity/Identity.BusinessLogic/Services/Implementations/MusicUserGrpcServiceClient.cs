using AutoMapper;
using Identity.BusinessLogic.Services.Interfaces;
using Identity.DataAccess.Entities;
using MusicService.Grpc;

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
            CancellationToken cancellationToken = default)
        {
            var request = _mapper.Map<AddUserRequest>(user);

            await _musicUserServiceClient.AddUserAsync(request, 
                cancellationToken: cancellationToken);

            return new AddUserResponse();
        }
    }
}
