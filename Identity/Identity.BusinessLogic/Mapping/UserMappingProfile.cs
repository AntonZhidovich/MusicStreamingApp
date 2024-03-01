using AutoMapper;
using Identity.BusinessLogic.GrpcClients;
using Identity.BusinessLogic.GrpcServers;
using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.TokenService;
using Identity.BusinessLogic.Models.UserService;
using Identity.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.BusinessLogic.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterUserRequest, User>();
            CreateMap<User, UserDto>();
            CreateMap<UpdateUserRequest, User>();
            CreateMap<UserDto, GetTokensRequest>().ForMember(s => s.Roles, opt => opt.Ignore());
            CreateMap<IdentityRole, RoleDto>();
            CreateMap<UserDto, UserInfo>();
            CreateMap<User, AddUserRequest>();
        }
    }
}
