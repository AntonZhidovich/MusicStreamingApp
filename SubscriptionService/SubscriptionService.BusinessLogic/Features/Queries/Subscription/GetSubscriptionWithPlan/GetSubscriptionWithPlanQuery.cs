using MediatR;
using SubscriptionService.BusinessLogic.Models;
using SubscriptionService.BusinessLogic.Models.Subscription;

namespace SubscriptionService.BusinessLogic.Features.Queries.GetSubscriptionWithPlan
{
    public class GetSubscriptionWithPlanQuery : IRequest<PageResponse<GetSubscriptionWithUserNameDto>>
    {
        public GetPageRequest PageRequest { get; set; }
        public string TariffPlanName { get; set; }

        public GetSubscriptionWithPlanQuery(GetPageRequest pageRequest, string tariffPlanName)
        {
            PageRequest = pageRequest;
            TariffPlanName = tariffPlanName;
        }
    }
}
