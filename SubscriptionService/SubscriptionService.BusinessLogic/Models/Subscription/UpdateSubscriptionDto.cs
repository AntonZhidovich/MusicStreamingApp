namespace SubscriptionService.BusinessLogic.Models.Subscription
{
    public class UpdateSubscriptionDto
    {
        public DateTime? NextFeeDate { get; set; }
        public double? Fee { get; set; }
    }
}
