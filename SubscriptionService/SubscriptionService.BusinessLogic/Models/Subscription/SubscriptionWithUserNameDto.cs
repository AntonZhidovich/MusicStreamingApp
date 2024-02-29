namespace SubscriptionService.BusinessLogic.Models.Subscription
{
    public class SubscriptionWithUserNameDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime SubscribedAt { get; set; }
        public DateTime NextFeeDate { get; set; }
        public string Type { get; set; }
        public double Fee { get; set; }
    }
}
