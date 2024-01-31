using AutoMapper;
using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.TokenService;
using Identity.BusinessLogic.Models.UserService;
using Identity.DataAccess.Entities;

namespace Identity.BusinessLogic.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterUserRequest, User>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, GetTokensRequest>().ForMember(s => s.Roles, opt => opt.Ignore());
        }
    }
}
