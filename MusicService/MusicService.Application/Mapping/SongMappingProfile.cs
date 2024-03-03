using AutoMapper;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.ReleaseService;
using MusicService.Application.Models.SongService;
using MusicService.Domain.Constants;
using MusicService.Domain.Entities;
using System.Globalization;

namespace MusicService.Application.Mapping
{
    public class SongMappingProfile : Profile
    {
        public SongMappingProfile()
        {
            CreateMap<UpdateSongRequest, Song>()
                .ForMember(dest => dest.Genres, options => options.Ignore())
                .ForMember(dest => dest.Title, options => options.MapFrom(src => src.Title))
                .ForMember(dest => dest.SourceName, options => options.MapFrom(src => src.SourceName))
                .ForAllMembers(options => options.Condition((source, dest, member) => member != null));

            CreateMap<AddSongToReleaseRequest, Song>()
                .ForMember(dest => dest.Title, options => options.MapFrom(src => src.Title))
                .ForMember(dest => dest.DurationMinutes, options =>
                options.MapFrom(src => TimeSpan.ParseExact(src.DurationMinutes, Constraints.timeSpanFormat, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Genres, options => options.MapFrom(src => new List<Genre>()))
                .ForMember(dest => dest.SourceName, options => options.MapFrom(src => src.SourceName));

            CreateMap<Song, SongDto>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, options => options.MapFrom(src => src.Title))
                .ForMember(dest => dest.SourceName, options => options.MapFrom(src => src.SourceName))
                .ForMember(dest => dest.DurationMinutes, options => options.MapFrom(src => src.DurationMinutes.ToString()))
                .ForMember(dest => dest.Genres, options => options.MapFrom(src => src.Genres.Select(genre => genre.Name)))
                .ForMember(dest => dest.Release, options => options.MapFrom(src => src.Release));

            CreateMap<Song, SongShortDto>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, options => options.MapFrom(src => src.Title))
                .ForMember(dest => dest.DurationMinutes, options => options.MapFrom(src => src.DurationMinutes.ToString()));
        }
    }
}
