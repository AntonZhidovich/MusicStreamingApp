namespace MusicService.Application.Models.DTOs
{
    public class PlaylistFullDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<SongDto> Songs { get; set; }
    }
}
