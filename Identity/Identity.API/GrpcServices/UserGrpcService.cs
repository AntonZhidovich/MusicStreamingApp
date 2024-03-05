using AutoMapper;
using Grpc.Core;
using Identity.BusinessLogic.Models.UserService;
using Identity.BusinessLogic.Services.Interfaces;
using Identity.Grpc;

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

        public override async Task<UserWithIdExistsResponse> UserWithIdExists(UserWithIdExistsRequest request, 
            ServerCallContext context)
        {
            var response = new UserWithIdExistsResponse
            {
                UserExists = await _userService.UserWithIdExists(request.Id)
            };

            return response;
        }

        public override async Task<GetIdUserNameMapResponse> GetIdUserNameMap(GetIdUserNameMapRequest request, ServerCallContext context)
        {
            var response = new GetIdUserNameMapResponse();

            var users = await _userService.GetByIdAsync(request.Ids);

            foreach (var user in users)
            {
                response.UsernamesById.Add(user.Id, user.UserName);
            }

            return response;
        }

        public override async Task<UserIsInRoleResponse> UserIsInRole(UserIsInRoleRequest request, ServerCallContext context)
        {
            var roles = await _userService.GetRolesAsync(request.Id);

            return new UserIsInRoleResponse { UserIsInRole = roles.Contains(request.RoleName) };
        }

        public override async Task<GetUserInfoResponse> GetUserInfo(GetUserInfoRequest request, ServerCallContext context)
        {
            UserInfo? userInfo = null;

            if (await _userService.UserWithIdExists(request.Id))
            {
                var userDto = await _userService.GetByIdAsync(request.Id);

                userInfo = _mapper.Map<UserInfo>(userDto);
            }

            return new GetUserInfoResponse { UserInfo = userInfo };
        }
    }
}
