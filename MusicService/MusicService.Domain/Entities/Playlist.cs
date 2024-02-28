namespace MusicService.Domain.Entities
{
    public class Playlist
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> SongIds { get; set; }
    }
}
