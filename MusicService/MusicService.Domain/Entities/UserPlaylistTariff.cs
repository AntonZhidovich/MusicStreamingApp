namespace MusicService.Domain.Entities
{
    public class UserPlaylistTariff
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string TariffName { get; set; }
        public int MaxPlaylistCount { get; set; }
    }
}
