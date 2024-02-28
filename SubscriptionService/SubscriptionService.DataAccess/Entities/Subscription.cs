namespace SubscriptionService.DataAccess.Entities
{
    public class Subscription
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public TariffPlan TariffPlan { get; set; }
        public DateTime SubscribedAt {  get; set; }
        public DateTime NextFeeDate { get; set; }
        public string Type { get; set; }
        public double Fee { get; set; }
    }
}
