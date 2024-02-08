using MusicService.Domain.Entities;

namespace MusicService.Application.Models.ReleaseService
{
    public class UpdateReleaseRequest
    {
        public string Name { get; set; }
        public DateTime ReleasedAt { get; set; }
    }
}
