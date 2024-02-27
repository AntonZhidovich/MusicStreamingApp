namespace MusicService.Application.Models.Messages
{
    public class SubscriptionMadeMessage
    {
        public string UserName { get; set; }
        public int MaxPlaylistCount { get; set; }
    }
}
