using MediatR;

namespace SubscriptionService.BusinessLogic.Features.Commands.DeleteTariffPlan
{
    public class DeleteTariffPlanCommand : IRequest
    {
        public string Id { get; set; }

        public DeleteTariffPlanCommand(string id)
        {
            Id = id;
        }
    }
}
