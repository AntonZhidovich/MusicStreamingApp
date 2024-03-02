using AutoMapper;
using MusicService.Application.Models.AuthorService;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.Messages;
using MusicService.Domain.Entities;
using MusicService.Grpc;

namespace MusicService.Application.Mapping
{
    public class AuthorMappingProfile : Profile
    {
        public AuthorMappingProfile()
        {
            CreateMap<Author, AuthorDto>()
               .ForMember(dest => dest.UserNames, options => options.MapFrom(src => src.Users.Select(user => user.UserName)));

            CreateMap<CreateAuthorRequest, Author>()
                .ForMember(dest => dest.Users, options => options.Ignore())
                .ForMember(dest => dest.Releases, options => options.Ignore());

            CreateMap<UpdateAuthorRequest, Author>()
                .ForMember(dest => dest.BrokenAt, options => options.MapFrom(src => src.BrokenAt ?? DateTime.Now))
                .ForAllMembers(options => options.Condition((source, dest, member) => member != null));

            CreateMap<UserUpdatedMessage, User>()
                .ForMember(dest => dest.UserName, options => options.MapFrom(src => src.NewUserName));

            CreateMap<AddUserRequest, User>();
        }
    }
}
