namespace MusicService.Domain.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public Author? Author { get; set; }
        public string? AuthorId { get; set; }
    }
}
