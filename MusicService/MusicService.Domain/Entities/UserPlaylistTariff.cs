namespace MusicService.Domain.Entities
{
    public class UserPlaylistTariff
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public int MaxPlaylistCount { get; set; }
    }
}
