namespace SubscriptionService.BusinessLogic.Models.Subscription
{
    public class CreateSubscriptionDto
    {
        public string UserName { get; set; }
        public string TariffPlanId { get; set; }
        public string Type { get; set; }
    }
}
