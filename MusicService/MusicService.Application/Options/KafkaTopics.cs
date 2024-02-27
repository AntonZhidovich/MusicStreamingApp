namespace MusicService.Application.Options
{
    public class KafkaTopics
    {
        public string UserDeleted { get; set; }
        public string UserUpdated { get; set; }
        public string SubscriptionMade { get; set; }
        public string SubscriptionCanceled { get; set; }
    }
}
