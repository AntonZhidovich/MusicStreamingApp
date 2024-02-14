using AutoMapper;
using MusicService.Application.Models.AuthorService;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.PlaylistService;
using MusicService.Application.Models.ReleaseService;
using MusicService.Application.Models.SongService;
using MusicService.Domain.Entities;
using System.Globalization;

namespace MusicService.Application.Mapping
{
    public class MappingProfile : Profile
    {
        private const string timeSpanFormat = "hh\\:mm\\:ss";

        public MappingProfile()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.UserNames, options => options.MapFrom(src => src.Users.Select(user => user.UserName)));
           
            CreateMap<CreateAuthorRequest, Author>()
                .ForMember(dest => dest.Users, options => options.Ignore())
                .ForMember(dest => dest.Releases, options => options.Ignore());

            CreateMap<UpdateAuthorRequest, Author>()
                .ForAllMembers(options => options.Condition((source, dest, member) => member != null));

            CreateMap<UpdateSongRequest, Song>()
                .ForMember(dest => dest.Genres, options => options.Ignore())
                .ForAllMembers(options => options.Condition((source, dest, member) => member != null));

            CreateMap<Release, ReleaseInSongDto>()
                .ForMember(dest => dest.Authors, options => options.MapFrom(src => src.Authors.Select(author => author.Name)));

            CreateMap<Song, SongDto>()
                .ForMember(dest => dest.DurationMinutes, options => options.MapFrom(src => src.DurationMinutes.ToString()))
                .ForMember(dest => dest.Genres, options => options.MapFrom(src => src.Genres.Select(genre => genre.Name)))
                .ForMember(dest => dest.Release, options => options.MapFrom(src => src.Release));

            CreateMap<Genre, GenreDto>();

            CreateMap<AddSongToReleaseRequest, Song>()
                .ForMember(dest => dest.DurationMinutes, options => 
                options.MapFrom(src => TimeSpan.ParseExact(src.DurationMinutes, timeSpanFormat, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Genres, options => options.MapFrom(src => new List<Genre>()));

            CreateMap<CreateReleaseRequest, Release>()
                .ForMember(dest => dest.Songs, options => options.MapFrom(src => new List<Song>()))
                .ForMember(dest => dest.Authors, options => options.MapFrom(src => new List<Author>()))
                .ForMember(dest => dest.SongsCount, options => options.Ignore());

            CreateMap<Song, SongInReleaseDto>();

            CreateMap<Release, ReleaseDto>()
                .ForMember(dest => dest.AuthorNames, options => options.MapFrom(src => src.Authors.Select(author => author.Name)))
                .ForMember(dest => dest.DurationMinutes, options => options.MapFrom(src => src.DurationMinutes.ToString()));

            CreateMap<Release, ReleaseShortDto>()
                .ForMember(dest => dest.AuthorNames, options => options.MapFrom(src => src.Authors.Select(author => author.Name)))
                .ForMember(dest => dest.DurationMinutes, options => options.MapFrom(src => src.DurationMinutes.ToString()));

            CreateMap<UpdateReleaseRequest, Release>()
                .ForAllMembers(options => options.Condition((source, dest, member) => member != null));

            CreateMap<Playlist, ShortPlaylistDto>()
                .ForMember(dest => dest.SongsCount, options => options.MapFrom(source => source.SongIds.Count));

            CreateMap<CreatePlaylistRequest, Playlist>();
            CreateMap<Playlist, FullPlaylistDto>();
        }
    }
}
