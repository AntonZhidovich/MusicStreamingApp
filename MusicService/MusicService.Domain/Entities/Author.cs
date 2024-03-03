namespace MusicService.Domain.Entities
{
    public class Author
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
        public List<Release> Releases { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsBroken { get; set; }
        public DateTime BrokenAt { get; set; }
        public string Description { get; set; }
    }
}
