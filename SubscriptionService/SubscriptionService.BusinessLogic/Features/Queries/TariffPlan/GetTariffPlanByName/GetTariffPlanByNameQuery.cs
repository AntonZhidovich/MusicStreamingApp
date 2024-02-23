using MediatR;
using SubscriptionService.BusinessLogic.Models.TariffPlan;

namespace SubscriptionService.BusinessLogic.Features.Queries.GetTariffPlanByName
{
    public class GetTariffPlanByNameQuery : IRequest<GetTariffPlanDto>
    {
        public string Name { get; set; }

        public GetTariffPlanByNameQuery(string name)
        {
            Name = name;
        }
    }
}
