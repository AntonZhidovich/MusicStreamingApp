using MediatR;

namespace SubscriptionService.BusinessLogic.Commands.CancelSubscription
{
    public class CancelSubscriptionCommand : IRequest
    {
        public string SubscriptionId { get; set; }

        public CancelSubscriptionCommand(string subscriptionId)
        {
            SubscriptionId = subscriptionId;
        }
    }
}
