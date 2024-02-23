using FluentValidation;
using SubscriptionService.BusinessLogic.Features.Commands.UpdateSubscription;

namespace SubscriptionService.BusinessLogic.Validators
{
    public class UpdateSubscriptionValidator : AbstractValidator<UpdateSubscriptionCommand>
    {
        public UpdateSubscriptionValidator()
        {
            RuleFor(command => command.Dto.NextFeeDate)
                .GreaterThan(DateTime.UtcNow);
        }
    }
}