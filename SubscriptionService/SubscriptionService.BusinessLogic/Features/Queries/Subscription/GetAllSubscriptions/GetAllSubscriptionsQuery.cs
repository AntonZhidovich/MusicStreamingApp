using MediatR;
using SubscriptionService.BusinessLogic.Models;
using SubscriptionService.BusinessLogic.Models.Subscription;

namespace SubscriptionService.BusinessLogic.Features.Queries.GetAllSubscriptions
{
    public class GetAllSubscriptionsQuery : IRequest<PageResponse<SubscriptionWithUserNameDto>>
    {
        public GetPageRequest GetPageRequest { get; set; }

        public GetAllSubscriptionsQuery(GetPageRequest getPageRequest)
        {
            GetPageRequest = getPageRequest;
        }
    }
}
