using FluentValidation;
using Identity.BusinessLogic.Constants;
using Identity.BusinessLogic.Models.UserService;

namespace Identity.BusinessLogic.Validators
{
    public class CheckPasswordValidator : AbstractValidator<CheckPasswordRequest>
    {
        public CheckPasswordValidator()
        {
            RuleFor(dto => dto.Email)
                .NotEmpty()
                .Length(Constraints.minEmailLength, Constraints.maxEmailLength)
                .EmailAddress();

            RuleFor(dto => dto.Password)
                .NotEmpty()
                .Length(Constraints.minPasswordLength, Constraints.maxPasswordLength);
        }
    }
}
