namespace MusicService.Domain.Entities
{
    public class Release
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Author> Authors { get; set; }
        public List<Song> Songs { get; set; }
        public int SongsCount { get; set; }
        public int DurationMinutes { get; set; }
    }
}
