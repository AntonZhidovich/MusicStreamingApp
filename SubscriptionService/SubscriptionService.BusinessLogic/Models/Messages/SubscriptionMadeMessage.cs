namespace SubscriptionService.BusinessLogic.Models.Messages
{
    public class SubscriptionMadeMessage
    {
        public string UserId { get; set; }
        public int MaxPlaylistCount { get; set; }
    }
}
