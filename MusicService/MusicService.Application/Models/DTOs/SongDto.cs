using MusicService.Application.Models.SongService;

namespace MusicService.Application.Models.DTOs
{
    public class SongDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<string> Genres { get; set; }
        public ReleaseInSongDto Release { get; set; }
        public int DurationMinutes { get; set; }
    }
}
