namespace MusicService.Application.Models.DTOs
{
    public class ReleaseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime ReleasedAt { get; set; }
        public List<string> AuthorNames { get; set; }
        public List<SongShortDto> Songs { get; set; }
        public int SongsCount { get; set; }
        public string DurationMinutes { get; set; }
    }
}
