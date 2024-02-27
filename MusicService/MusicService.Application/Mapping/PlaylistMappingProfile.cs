using AutoMapper;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.PlaylistService;
using MusicService.Domain.Entities;

namespace MusicService.Application.Mapping
{
    public class PlaylistMappingProfile : Profile
    {
        public PlaylistMappingProfile()
        {
            CreateMap<Playlist, PlaylistShortDto>()
                .ForMember(dest => dest.SongsCount, options => options.MapFrom(source => source.SongIds.Count));

            CreateMap<CreatePlaylistRequest, Playlist>();
            CreateMap<Playlist, PlaylistFullDto>();
        }
    }
}
