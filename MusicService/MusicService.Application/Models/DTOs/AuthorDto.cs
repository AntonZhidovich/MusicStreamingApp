using MusicService.Domain.Entities;

namespace MusicService.Application.Models.DTOs
{
    public class AuthorDto
    {
        public string Name { get; set; }
        public List<string> UserNames { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByUsername { get; set; }
        public bool IsBroken { get; set; }
        public DateTime BrokenAt { get; set; }
        public string Description { get; set; }
    }
}
