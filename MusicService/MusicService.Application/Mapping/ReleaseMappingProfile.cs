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
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, options => options.MapFrom(src => src.Name))
                .ForMember(dest => dest.Authors, options => options.MapFrom(src => src.Authors.Select(author => author.Name)));

            CreateMap<CreateReleaseRequest, Release>()
                .ForMember(dest => dest.Name, options => options.MapFrom(src => src.Name))
                .ForMember(dest => dest.ReleasedAt, options => options.MapFrom(src => src.ReleasedAt))
                .ForMember(dest => dest.Songs, options => options.MapFrom(src => new List<Song>()))
                .ForMember(dest => dest.Authors, options => options.MapFrom(src => new List<Author>()))
                .ForMember(dest => dest.SongsCount, options => options.Ignore());

            CreateMap<Release, ReleaseDto>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, options => options.MapFrom(src => src.Name))
                .ForMember(dest => dest.Type, options => options.MapFrom(src => src.Type))
                .ForMember(dest => dest.ReleasedAt, options => options.MapFrom(src => src.ReleasedAt))
                .ForMember(dest => dest.AuthorNames, options => options.MapFrom(src => src.Authors.Select(author => author.Name)))
                .ForMember(dest => dest.Songs, options => options.MapFrom(src => src.Songs))
                .ForMember(dest => dest.SongsCount, options => options.MapFrom(src => src.SongsCount))
                .ForMember(dest => dest.DurationMinutes, options => options.MapFrom(src => src.DurationMinutes.ToString()));

            CreateMap<Release, ReleaseShortDto>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, options => options.MapFrom(src => src.Name))
                .ForMember(dest => dest.Type, options => options.MapFrom(src => src.Type))
                .ForMember(dest => dest.ReleasedAt, options => options.MapFrom(src => src.ReleasedAt))
                .ForMember(dest => dest.AuthorNames, options => options.MapFrom(src => src.Authors.Select(author => author.Name)))
                .ForMember(dest => dest.SongsCount, options => options.MapFrom(src => src.SongsCount))
                .ForMember(dest => dest.DurationMinutes, options => options.MapFrom(src => src.DurationMinutes.ToString()));

            CreateMap<UpdateReleaseRequest, Release>()
                .ForMember(dest => dest.Name, options => options.MapFrom(src => src.Name))
                .ForMember(dest => dest.ReleasedAt, options => options.MapFrom(src => src.ReleasedAt))
                .ForAllMembers(options => options.Condition((source, dest, member) => member != null));
        }
    }
}
