namespace MusicService.Application.Models.SongService
{
    public class ReleaseInSongDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Authors { get; set; } 
    }
}
