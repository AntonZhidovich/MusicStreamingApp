using FluentValidation;
using SubscriptionService.BusinessLogic.Features.Commands.UpdateTariffPlan;
using SubscriptionService.DataAccess.Constants;

namespace SubscriptionService.BusinessLogic.Validators
{
    public class UpdateTariffPlanValidator : AbstractValidator<UpdateTariffPlanCommand>
    {
        public UpdateTariffPlanValidator()
        {
            RuleFor(command => command.Dto.Name)
                .MaximumLength(Constraints.tariffPlanNameMaxLength);

            RuleFor(command => command.Dto.Description)
                .MaximumLength(Constraints.descpriptionMaxLength);
        }
    }
}