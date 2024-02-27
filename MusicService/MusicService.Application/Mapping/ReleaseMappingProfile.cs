using AutoMapper;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.ReleaseService;
using MusicService.Application.Models.SongService;
using MusicService.Domain.Constants;
using MusicService.Domain.Entities;
using System.Globalization;

namespace MusicService.Application.Mapping
{
    public class ReleaseMappingProfile : Profile
    {
        public ReleaseMappingProfile()
        {
            CreateMap<Release, ReleaseInSongDto>()
                .ForMember(dest => dest.Authors, options => options.MapFrom(src => src.Authors.Select(author => author.Name)));

            CreateMap<AddSongToReleaseRequest, Song>()
                .ForMember(dest => dest.DurationMinutes, options =>
                options.MapFrom(src => TimeSpan.ParseExact(src.DurationMinutes, Constraints.timeSpanFormat, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Genres, options => options.MapFrom(src => new List<Genre>()));

            CreateMap<CreateReleaseRequest, Release>()
                .ForMember(dest => dest.Songs, options => options.MapFrom(src => new List<Song>()))
                .ForMember(dest => dest.Authors, options => options.MapFrom(src => new List<Author>()))
                .ForMember(dest => dest.SongsCount, options => options.Ignore());

            CreateMap<Release, ReleaseDto>()
                .ForMember(dest => dest.AuthorNames, options => options.MapFrom(src => src.Authors.Select(author => author.Name)))
                .ForMember(dest => dest.DurationMinutes, options => options.MapFrom(src => src.DurationMinutes.ToString()));

            CreateMap<Release, ReleaseShortDto>()
                .ForMember(dest => dest.AuthorNames, options => options.MapFrom(src => src.Authors.Select(author => author.Name)))
                .ForMember(dest => dest.DurationMinutes, options => options.MapFrom(src => src.DurationMinutes.ToString()));

            CreateMap<UpdateReleaseRequest, Release>()
                .ForAllMembers(options => options.Condition((source, dest, member) => member != null));
        }
    }
}
