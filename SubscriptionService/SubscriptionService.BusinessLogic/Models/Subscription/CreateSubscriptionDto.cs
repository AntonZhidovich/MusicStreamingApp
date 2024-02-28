namespace SubscriptionService.BusinessLogic.Models.Subscription
{
    public class CreateSubscriptionDto
    {
        public string UserId { get; set; }
        public string TariffPlanId { get; set; }
        public string Type { get; set; }
    }
}
