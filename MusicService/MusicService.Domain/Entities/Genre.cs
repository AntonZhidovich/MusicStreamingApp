namespace MusicService.Domain.Entities
{
    public class Genre
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Song> Songs { get; set; }
    }
}
