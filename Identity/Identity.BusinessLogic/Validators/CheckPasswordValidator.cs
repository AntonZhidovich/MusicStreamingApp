using FluentValidation;
using Identity.BusinessLogic.Models.UserService;

namespace Identity.BusinessLogic.Validators
{
    public class CheckPasswordValidator : AbstractValidator<CheckPasswordRequest>
    {
        private const int maximumLength = 30;
        private const int minimumLength = 5;

        public CheckPasswordValidator()
        {
            RuleFor(dto => dto.Email)
                .NotEmpty()
                .Length(minimumLength, maximumLength)
                .EmailAddress();

            RuleFor(dto => dto.Password)
                .NotEmpty()
                .Length(minimumLength, maximumLength);
        }
    }
}
