using AutoMapper;
using MusicService.Application.Models.DTOs;
using MusicService.Domain.Entities;

namespace MusicService.Application.Mapping
{
    public class GenreMappingProfile : Profile
    {
        public GenreMappingProfile()
        {
            CreateMap<Genre, GenreDto>()
                .ForMember(dest => dest.Name, options => options.MapFrom(source => source.Name))
                .ForMember(dest => dest.Description, options => options.MapFrom(source => source.Description));
        }
    }
}
