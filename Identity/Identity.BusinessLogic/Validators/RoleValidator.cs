using FluentValidation;
using Identity.BusinessLogic.Models;

namespace Identity.BusinessLogic.Validators
{
    public class RoleValidator : AbstractValidator<RoleDto>
    {
        private const int maximumLength = 20;

        public RoleValidator()
        {
            RuleFor(role => role.RoleName)
                .NotEmpty()
                .MaximumLength(maximumLength);
        }
    }
}
