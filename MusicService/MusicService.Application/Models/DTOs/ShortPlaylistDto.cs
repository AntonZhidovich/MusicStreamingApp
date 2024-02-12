namespace MusicService.Application.Models.DTOs
{
    public class ShortPlaylistDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int SongsCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
