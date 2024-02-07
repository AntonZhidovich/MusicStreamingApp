namespace MusicService.Domain.Entities
{
    public class Song
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<Genre> Genres { get; set; }
        public Release Release {  get; set; }
        public int DurationMinutes { get; set; }
        public string SourceName { get; set; }
    }
}
