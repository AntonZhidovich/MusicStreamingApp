using FluentValidation;
using SubscriptionService.BusinessLogic.Models.Subscription;

namespace SubscriptionService.BusinessLogic.Validators
{
    public class UpdateSubscriptionValidator : AbstractValidator<UpdateSubscriptionDto>
    {
        public UpdateSubscriptionValidator()
        {
            RuleFor(subscription => subscription.NextFeeDate)
                .GreaterThan(DateTime.UtcNow);
        }
    }
}