using AutoMapper;
using MusicService.Application.Models.AuthorService;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.SongService;
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

            CreateMap<UpdateSongRequest, Song>()
                .ForMember(dest => dest.Genres, options => options.Ignore());

            CreateMap<Release, ReleaseInSongDto>()
                .ForMember(dest => dest.Authors, options => options.MapFrom(src => src.Authors.Select(author => author.Name)));

            CreateMap<Song, SongDto>()
                .ForMember(dest => dest.Genres, options => options.MapFrom(src => src.Genres.Select(genre => genre.Name)))
                .ForMember(dest => dest.Release, options => options.MapFrom(src => src.Release));

            CreateMap<Genre, GenreDto>();
        }
    }
}
