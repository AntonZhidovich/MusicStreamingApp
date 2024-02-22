using FluentValidation;
using SubscriptionService.BusinessLogic.Models.TariffPlan;
using SubscriptionService.DataAccess.Constants;

namespace SubscriptionService.BusinessLogic.Validators
{
    public class UpdateTariffPlanValidator : AbstractValidator<UpdateTariffPlanDto>
    {
        public UpdateTariffPlanValidator()
        {
            RuleFor(plan => plan.Name)
                .MaximumLength(Constraints.tariffPlanNameMaxLength);

            RuleFor(plan => plan.Description)
                .MaximumLength(Constraints.descpriptionMaxLength);
        }
    }
}