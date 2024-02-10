namespace MusicService.Application.Models.AuthorService
{
    public class UpdateAuthorRequest
    {
        public string? Description { get; set; }
        public bool? IsBroken { get; set; }
        public DateTime? BrokenAt { get; set; } = DateTime.Now;
    }
}
