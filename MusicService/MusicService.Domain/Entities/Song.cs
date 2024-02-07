namespace MusicService.Domain.Entities
{
    public class Song
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<Genre> Genre { get; set; }
        public Release Release {  get; set; }
        public TimeSpan Duration { get; set; }
        public string SourceName { get; set; }
    }
}
