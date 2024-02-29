using AutoMapper;
using Grpc.Core;
using Identity.BusinessLogic.GrpcServers;
using Identity.BusinessLogic.Services.Interfaces;

namespace Identity.API.GrpcServices
{
    public class UserGrpcService : UserService.UserServiceBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserGrpcService(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public override async Task<UserWithIdExistsResponse> UserWithIdExists(UserWithIdExistsRequest request, ServerCallContext context)
        {
            var response = new UserWithIdExistsResponse
            {
                UserExists = await _userService.UserWithIdExists(request.Id)
            };

            return response;
        }

        public override async Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
        {
            var response = new GetUserByIdResponse();

            try
            {
                var user = await _userService.GetByIdAsync(request.Id);

                response.User = _mapper.Map<UserInfo>(user);
            }
            catch (Exception exception)
            {
                response.Error = exception.Message;
            }

            return response;
        }

        public override async Task<GetUsersByIdResponse> GetUsersById(GetUsersByIdRequest request, ServerCallContext context)
        {
            var response = new GetUsersByIdResponse();

            var users = await _userService.GetByIdAsync(request.Ids);

            response.Users.AddRange(_mapper.Map<IEnumerable<UserInfo>>(users));

            return response;
        }
    }
}
