using FluentValidation;
using SubscriptionService.BusinessLogic.Models.Subscription;
using SubscriptionService.DataAccess.Constants;

namespace SubscriptionService.BusinessLogic.Validators
{
    public class CreateSubscriptionValidator : AbstractValidator<CreateSubscriptionDto>
    {
        public CreateSubscriptionValidator()
        {
            RuleFor(subscription => subscription.UserName)
                .NotEmpty()
                .MaximumLength(Constraints.usernameMaxLength);

            RuleFor(subscription => subscription.TariffPlanId)
                .NotEmpty()
                .MaximumLength(Constraints.idMaxLength);

            RuleFor(subscription => subscription.Type)
                .NotEmpty()
                .MaximumLength(Constraints.subscriptionTypeMaxLength);
        }
    }
}