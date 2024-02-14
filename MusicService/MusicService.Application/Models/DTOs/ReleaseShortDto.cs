namespace MusicService.Application.Models.DTOs
{
    public class ReleaseShortDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime ReleasedAt { get; set; }
        public List<string> AuthorNames { get; set; }
        public int SongsCount { get; set; }
        public string DurationMinutes { get; set; }
    }
}
