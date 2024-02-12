namespace MusicService.Application.Models.AuthorService
{
    public class CreateAuthorRequest
    {
        public string Name { get; set; }
        public List<string> UserNames { get; set; }
        public string? Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsBroken { get; set; } = false;
        public DateTime BrokenAt { get; set; } = DateTime.MinValue;
    }
}