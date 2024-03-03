using FluentValidation;
using Identity.BusinessLogic.Constants;
using Identity.BusinessLogic.Models;
using System.Data;

namespace Identity.BusinessLogic.Validators
{
    public class RoleValidator : AbstractValidator<RoleDto>
    {
        public RoleValidator()
        {
            RuleFor(role => role.Name)
                .NotEmpty()
                .MaximumLength(Constraints.maxRoleNameLength);
        }
    }
}
