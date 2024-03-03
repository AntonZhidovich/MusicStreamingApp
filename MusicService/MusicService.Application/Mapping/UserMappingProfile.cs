using AutoMapper;
using MusicService.Application.Models.Messages;
using MusicService.Domain.Entities;
using MusicService.Grpc;

namespace MusicService.Application.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserUpdatedMessage, User>()
                .ForMember(dest => dest.UserName, options => options.MapFrom(src => src.NewUserName))
                .ForMember(dest => dest.Id, options => options.Ignore());

            CreateMap<AddUserRequest, User>()
                .ForMember(dest => dest.UserName, options => options.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id));
        }
    }
}
