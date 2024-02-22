using MediatR;
using SubscriptionService.BusinessLogic.Models.Subscription;

namespace SubscriptionService.BusinessLogic.Commands.MakeSubscription
{
    public class MakeSubscriptionCommand : IRequest<GetSubscriptionDto>
    {
        public CreateSubscriptionDto Dto { get; set; }

        public MakeSubscriptionCommand(CreateSubscriptionDto dto)
        {
            Dto = dto;
        }
    }
}
