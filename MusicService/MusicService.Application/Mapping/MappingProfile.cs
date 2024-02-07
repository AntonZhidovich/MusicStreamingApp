using AutoMapper;
using MusicService.Application.Models;
using MusicService.Application.Models.AuthorService;
using MusicService.Domain.Entities;

namespace MusicService.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.UserNames, options => options.MapFrom(src => src.Users.Select(user => user.UserName)));
            CreateMap<CreateAuthorRequest, Author>()
                .ForMember(dest => dest.Users, options => options.Ignore())
                .ForMember(dest => dest.Releases, options => options.Ignore());
        }
    }
}
