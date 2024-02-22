using MediatR;
using SubscriptionService.BusinessLogic.Models.Subscription;

namespace SubscriptionService.BusinessLogic.Queries.GetUserSubscription
{
    public class GetUserSubscriptionQuery : IRequest<GetSubscriptionDto>
    {
        public string UserName { get; set; }

        public GetUserSubscriptionQuery(string userName)
        {
            UserName = userName;
        }
    }
}
