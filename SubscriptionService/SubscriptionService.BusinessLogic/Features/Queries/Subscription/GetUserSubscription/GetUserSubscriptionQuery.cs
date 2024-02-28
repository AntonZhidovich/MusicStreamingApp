using MediatR;
using SubscriptionService.BusinessLogic.Models.Subscription;

namespace SubscriptionService.BusinessLogic.Features.Queries.GetUserSubscription
{
    public class GetUserSubscriptionQuery : IRequest<GetSubscriptionDto>
    {
        public string UserId { get; set; }

        public GetUserSubscriptionQuery(string userId)
        {
            UserId = userId;
        }
    }
}
