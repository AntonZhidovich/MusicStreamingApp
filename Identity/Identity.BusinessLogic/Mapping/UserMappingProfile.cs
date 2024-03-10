using AutoMapper;
using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.TokenService;
using Identity.BusinessLogic.Models.UserService;
using Identity.DataAccess.Entities;
using Identity.Grpc;
using MusicService.Grpc;

namespace Identity.BusinessLogic.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterUserRequest, User>()
                .ForMember(dest => dest.FirstName, options => options.MapFrom(source => source.FirstName))
                .ForMember(dest => dest.LastName, options => options.MapFrom(source => source.LastName))
                .ForMember(dest => dest.Email, options => options.MapFrom(source => source.Email))
                .ForMember(dest => dest.UserName, options => options.MapFrom(source => source.UserName))
                .ForMember(dest => dest.CreationDate, options => options.MapFrom(source => source.CreationDate))
                .ForMember(dest => dest.Region, options => options.MapFrom(source => source.Region));

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Id, options => options.MapFrom(source => source.Id))
                .ForMember(dest => dest.FirstName, options => options.MapFrom(source => source.FirstName))
                .ForMember(dest => dest.LastName, options => options.MapFrom(source => source.LastName))
                .ForMember(dest => dest.Email, options => options.MapFrom(source => source.Email))
                .ForMember(dest => dest.UserName, options => options.MapFrom(source => source.UserName))
                .ForMember(dest => dest.CreationDate, options => options.MapFrom(source => source.CreationDate))
                .ForMember(dest => dest.Region, options => options.MapFrom(source => source.Region));

            CreateMap<UpdateUserRequest, User>()
                .ForMember(dest => dest.FirstName, options => options.MapFrom(source => source.FirstName))
                .ForMember(dest => dest.LastName, options => options.MapFrom(source => source.LastName))
                .ForMember(dest => dest.UserName, options => options.MapFrom(source => source.UserName))
                .ForMember(dest => dest.Region, options => options.MapFrom(source => source.Region));

            CreateMap<User, GetTokensRequest>()
                .ForMember(s => s.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.Id, options => options.MapFrom(source => source.Id))
                .ForMember(dest => dest.FirstName, options => options.MapFrom(source => source.FirstName))
                .ForMember(dest => dest.LastName, options => options.MapFrom(source => source.LastName))
                .ForMember(dest => dest.Email, options => options.MapFrom(source => source.Email))
                .ForMember(dest => dest.UserName, options => options.MapFrom(source => source.UserName));

            CreateMap<User, AddUserRequest>()
                .ForMember(dest => dest.Id, options => options.MapFrom(source => source.Id))
                .ForMember(dest => dest.UserName, options => options.MapFrom(source => source.UserName));

            CreateMap<UserDto, UserInfo>()
                .ForMember(dest => dest.FirstName, options => options.MapFrom(source => source.FirstName))
                .ForMember(dest => dest.LastName, options => options.MapFrom(source => source.LastName))
                .ForMember(dest => dest.Email, options => options.MapFrom(source => source.Email))
                .ForMember(dest => dest.Region, options => options.MapFrom(source => source.Region));
        }
    }
}
