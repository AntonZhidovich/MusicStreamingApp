using MusicService.Application.Models.SongService;
using MusicService.Domain.Entities;

namespace MusicService.Application.Models.DTOs
{
    public class SongDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<string> Genres { get; set; }
        public ReleaseInSongDto Release { get; set; }
        public TimeSpan Duration { get; set; }
        public string SourceName { get; set; }
    }
}
