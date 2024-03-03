using FluentValidation;
using SubscriptionService.BusinessLogic.Features.Commands.CreateTariffPlan;
using SubscriptionService.DataAccess.Constants;

namespace SubscriptionService.BusinessLogic.Validators
{
    public class CreateTariffPlanValidator : AbstractValidator<CreateTariffPlanCommand>
    {
        public CreateTariffPlanValidator()
        {
            RuleFor(command => command.Dto.Name)
                .NotEmpty()
                .MaximumLength(Constraints.tariffPlanNameMaxLength);

            RuleFor(command => command.Dto.Description)
                .MaximumLength(Constraints.descpriptionMaxLength);

            RuleFor(command => command.Dto.MaxPlaylistsCount)
                .GreaterThan(0)
                .NotEmpty();

            RuleFor(command => command.Dto.MonthFee)
                .GreaterThan(0)
                .NotEmpty();

            RuleFor(command => command.Dto.AnnualFee)
                .GreaterThan(0)
                .NotEmpty();
        }
    }
}
