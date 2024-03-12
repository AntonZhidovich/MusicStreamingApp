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
               .ForMember(dest => dest.Name, options => options.MapFrom(src => src.Name))
               .ForMember(dest => dest.UserNames, options => options.MapFrom(src => src.Users.Select(user => user.UserName)))
               .ForMember(dest => dest.CreatedAt, options => options.MapFrom(src => src.CreatedAt))
               .ForMember(dest => dest.IsBroken, options => options.MapFrom(src => src.IsBroken))
               .ForMember(dest => dest.BrokenAt, options => options.MapFrom(src => src.BrokenAt))
               .ForMember(dest => dest.Description, options => options.MapFrom(src => src.Description));

            CreateMap<CreateAuthorRequest, Author>()
                .ForMember(dest => dest.Name, options => options.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedAt, options => options.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.IsBroken, options => options.MapFrom(src => src.IsBroken))
                .ForMember(dest => dest.BrokenAt, options => options.MapFrom(src => src.BrokenAt))
                .ForMember(dest => dest.Description, options => options.MapFrom(src => src.Description))
                .ForMember(dest => dest.Users, options => options.Ignore())
                .ForMember(dest => dest.Releases, options => options.Ignore());

            CreateMap<UpdateAuthorRequest, Author>()
                .ForMember(dest => dest.IsBroken, options => options.MapFrom(src => src.IsBroken))
                .ForMember(dest => dest.BrokenAt, options => options.MapFrom(src => src.BrokenAt))
                .ForMember(dest => dest.Description, options => options.MapFrom(src => src.Description))
                .ForAllMembers(options => options.Condition((source, dest, member) => member != null));
        }
    }
}
