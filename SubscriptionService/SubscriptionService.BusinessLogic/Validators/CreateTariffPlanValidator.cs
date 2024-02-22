using FluentValidation;
using SubscriptionService.BusinessLogic.Models.TariffPlan;
using SubscriptionService.DataAccess.Constants;

namespace SubscriptionService.BusinessLogic.Validators
{
    public class CreateTariffPlanValidator : AbstractValidator<CreateTariffPlanDto>
    {
        public CreateTariffPlanValidator()
        {
            RuleFor(plan => plan.Name)
                .NotEmpty()
                .MaximumLength(Constraints.tariffPlanNameMaxLength);

            RuleFor(plan => plan.Description)
                .MaximumLength(Constraints.descpriptionMaxLength);

            RuleFor(plan => plan.MaxPlaylistsCount)
                .GreaterThan(0)
                .NotEmpty();

            RuleFor(plan => plan.MonthFee)
                .GreaterThan(0)
                .NotEmpty();

            RuleFor(plan => plan.AnnualFee)
                .GreaterThan(0)
                .NotEmpty();
        }
    }
}
