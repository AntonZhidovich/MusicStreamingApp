using MediatR;
using SubscriptionService.BusinessLogic.Features.Commands.MakeSubscriptionPayment;
using SubscriptionService.BusinessLogic.Features.Services.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Services.Implementations
{
    public class BackgroundJobsService : IBackgroundJobsService
    {
        private readonly ISender _mediator;

        public BackgroundJobsService(ISender mediator)
        {
            _mediator = mediator;
        }

        public void MakeSubscriptionPayment(string subscriptionId)
        {
            var command = new MakeSubscriptionPaymentCommand(subscriptionId);

            _mediator.Send(command).Wait();
        }
    }
}
