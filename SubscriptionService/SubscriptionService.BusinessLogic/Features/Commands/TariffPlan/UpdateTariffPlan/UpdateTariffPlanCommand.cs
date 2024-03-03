using MediatR;
using SubscriptionService.BusinessLogic.Models.TariffPlan;

namespace SubscriptionService.BusinessLogic.Features.Commands.UpdateTariffPlan
{
    public class UpdateTariffPlanCommand : IRequest<GetTariffPlanDto>
    {
        public string Id { get; set; }
        public UpdateTariffPlanDto Dto { get; set; }

        public UpdateTariffPlanCommand(string id, UpdateTariffPlanDto dto)
        {
            Id = id;
            Dto = dto;
        }
    }
}
