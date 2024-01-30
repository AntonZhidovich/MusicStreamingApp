using AutoMapper;
using Identity.BusinessLogic.Models;
using Identity.DataAccess.Entities;

namespace Identity.BusinessLogic.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterUserRequest, User>();
            CreateMap<User, GetUserDto>();
        }
    }
}
