using FluentValidation;
using SubscriptionService.BusinessLogic.Features.Commands.MakeSubscription;
using SubscriptionService.DataAccess.Constants;

namespace SubscriptionService.BusinessLogic.Validators
{
    public class CreateSubscriptionValidator : AbstractValidator<MakeSubscriptionCommand>
    {
        public CreateSubscriptionValidator()
        {
            RuleFor(command => command.Dto.UserName)
                .NotEmpty()
                .MaximumLength(Constraints.usernameMaxLength);

            RuleFor(command => command.Dto.TariffPlanId)
                .NotEmpty()
                .MaximumLength(Constraints.idMaxLength);

            RuleFor(command => command.Dto.Type)
                .NotEmpty()
                .MaximumLength(Constraints.subscriptionTypeMaxLength);
        }
    }
}