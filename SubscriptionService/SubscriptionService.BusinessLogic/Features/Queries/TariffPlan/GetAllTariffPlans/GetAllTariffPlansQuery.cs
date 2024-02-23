using MediatR;
using SubscriptionService.BusinessLogic.Models;
using SubscriptionService.BusinessLogic.Models.TariffPlan;

namespace SubscriptionService.BusinessLogic.Features.Queries.GetAllTariffPlans
{
    public class GetAllTariffPlansQuery : IRequest<PageResponse<GetTariffPlanDto>>
    {
        public GetPageRequest PageRequest { get; set; }

        public GetAllTariffPlansQuery(GetPageRequest pageRequest)
        {
            PageRequest = pageRequest;
        }
    }
}
