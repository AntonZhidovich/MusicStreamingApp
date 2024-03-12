using Identity.Grpc;

namespace SubscriptionService.BusinessLogic.Models.Subscription
{
    public class SubscriptionWithUserInfo
    {
        public GetSubscriptionDto Subscription { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
