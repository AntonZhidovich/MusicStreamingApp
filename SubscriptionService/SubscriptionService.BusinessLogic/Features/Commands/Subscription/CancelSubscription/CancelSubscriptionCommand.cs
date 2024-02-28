using MediatR;

namespace SubscriptionService.BusinessLogic.Features.Commands.CancelSubscription
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
