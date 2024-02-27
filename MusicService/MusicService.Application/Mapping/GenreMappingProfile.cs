using AutoMapper;
using MusicService.Application.Models.DTOs;
using MusicService.Domain.Entities;

namespace MusicService.Application.Mapping
{
    public class GenreMappingProfile : Profile
    {
        public GenreMappingProfile()
        {
            CreateMap<Genre, GenreDto>();
        }
    }
}
