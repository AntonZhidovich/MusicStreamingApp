using MediatR;
using SubscriptionService.BusinessLogic.Models.Subscription;

namespace SubscriptionService.BusinessLogic.Commands.UpdateSubscription
{
    public class UpdateSubscriptionCommand : IRequest<GetSubscriptionDto>
    {
        public string Id { get; set; }
        public UpdateSubscriptionDto Dto { get; set; }

        public UpdateSubscriptionCommand(string id, UpdateSubscriptionDto dto)
        {
            Id = id;
            Dto = dto;
        }

    }
}
