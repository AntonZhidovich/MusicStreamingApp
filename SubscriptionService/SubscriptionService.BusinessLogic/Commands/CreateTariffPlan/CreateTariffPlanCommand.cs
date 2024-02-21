using MediatR;
using SubscriptionService.BusinessLogic.Models.TariffPlan;

namespace SubscriptionService.BusinessLogic.Commands.CreateTariffPlan
{
    public class CreateTariffPlanCommand : IRequest<GetTariffPlanDto>
    {
        public CreateTariffPlanDto Dto { get; set; }

        public CreateTariffPlanCommand(CreateTariffPlanDto dto)
        {
            Dto = dto;
        }
    }
}
