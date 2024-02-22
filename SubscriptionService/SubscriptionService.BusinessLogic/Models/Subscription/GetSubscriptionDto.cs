namespace SubscriptionService.BusinessLogic.Models.Subscription
{
    public class GetSubscriptionDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string TariffPlanName { get; set; }
        public DateTime SubscribedAt { get; set; }
        public DateTime NextFeeDate { get; set; }
        public string Type { get; set; }
        public double Fee { get; set; }
    }
}
