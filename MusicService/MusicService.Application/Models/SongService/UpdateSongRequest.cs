namespace MusicService.Application.Models.SongService
{
    public class UpdateSongRequest
    {
        public string? Title { get; set; }
        public List<string>? Genres { get; set; }
        public string? SourceName { get; set; }
    }
}
