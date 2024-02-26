using AutoMapper;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.SongService;
using MusicService.Domain.Entities;

namespace MusicService.Application.Mapping
{
    public class SongMappingProfile : Profile
    {
        public SongMappingProfile()
        {
            CreateMap<UpdateSongRequest, Song>()
                .ForMember(dest => dest.Genres, options => options.Ignore())
                .ForAllMembers(options => options.Condition((source, dest, member) => member != null));

            CreateMap<Song, SongDto>()
                .ForMember(dest => dest.DurationMinutes, options => options.MapFrom(src => src.DurationMinutes.ToString()))
                .ForMember(dest => dest.Genres, options => options.MapFrom(src => src.Genres.Select(genre => genre.Name)))
                .ForMember(dest => dest.Release, options => options.MapFrom(src => src.Release));

            CreateMap<Song, SongShortDto>();
        }
    }
}
