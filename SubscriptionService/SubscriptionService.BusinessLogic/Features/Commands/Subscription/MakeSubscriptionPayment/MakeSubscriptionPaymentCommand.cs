using MediatR;
using SubscriptionService.BusinessLogic.Models.Subscription;

namespace SubscriptionService.BusinessLogic.Features.Commands.MakeSubscriptionPayment
{
    public class MakeSubscriptionPaymentCommand : IRequest<GetSubscriptionDto>
    {
        public string Id { get; set; }

        public MakeSubscriptionPaymentCommand(string id)
        {
            Id = id;
        }
    }
}
