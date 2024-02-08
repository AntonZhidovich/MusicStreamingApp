namespace MusicService.Application.Models.ReleaseService
{
    public class CreateReleaseRequest
    {
        public string Name { get; set; }
        public DateTime ReleasedAt { get; set; }
        public List<string> AuthorNames { get; set; }
        public List<AddSongToReleaseRequest> Songs { get; set; }
    }
}

