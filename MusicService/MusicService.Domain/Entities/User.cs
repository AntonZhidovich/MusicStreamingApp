namespace MusicService.Domain.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Region { get; set; }
        public Author? Author { get; set; }
        public string? AuthorId { get; set; }
        public List<string> Roles { get; set; }
    }
}
