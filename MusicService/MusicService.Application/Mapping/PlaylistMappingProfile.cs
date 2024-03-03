using AutoMapper;
using MusicService.Application.Models.DTOs;
using MusicService.Application.Models.Messages;
using MusicService.Application.Models.PlaylistService;
using MusicService.Domain.Entities;

namespace MusicService.Application.Mapping
{
    public class PlaylistMappingProfile : Profile
    {
        public PlaylistMappingProfile()
        {
            CreateMap<Playlist, PlaylistShortDto>()
                .ForMember(dest => dest.Id, options => options.MapFrom(source => source.Id))
                .ForMember(dest => dest.Name, options => options.MapFrom(source => source.Name))
                .ForMember(dest => dest.CreatedAt, options => options.MapFrom(source => source.CreatedAt))
                .ForMember(dest => dest.SongsCount, options => options.MapFrom(source => source.SongIds.Count));

            CreateMap<CreatePlaylistRequest, Playlist>()
                .ForMember(dest => dest.Id, options => options.MapFrom(source => source.Id))
                .ForMember(dest => dest.Name, options => options.MapFrom(source => source.Name))
                .ForMember(dest => dest.CreatedAt, options => options.MapFrom(source => source.CreatedAt))
                .ForMember(dest => dest.SongIds, options => options.MapFrom(source => source.SongIds));

            CreateMap<Playlist, PlaylistFullDto>()
                .ForMember(dest => dest.Id, options => options.MapFrom(source => source.Id))
                .ForMember(dest => dest.Name, options => options.MapFrom(source => source.Name))
                .ForMember(dest => dest.UserId, options => options.MapFrom(source => source.UserId))
                .ForMember(dest => dest.CreatedAt, options => options.MapFrom(source => source.CreatedAt));

            CreateMap<SubscriptionMadeMessage, UserPlaylistTariff>()
                .ForMember(dest => dest.MaxPlaylistCount, options => options.MapFrom(source => source.MaxPlaylistCount))
                .ForMember(dest => dest.UserId, options => options.MapFrom(source => source.UserId));      
        }
    }
}
