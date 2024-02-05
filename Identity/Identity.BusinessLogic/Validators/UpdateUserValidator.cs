using FluentValidation;
using Identity.BusinessLogic.Models.UserService;

namespace Identity.BusinessLogic.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
    {
        private const int maximumLength = 30;
        private const int minimumLength = 5;

        public UpdateUserValidator()
        {
            RuleFor(user => user.FirstName)
                .NotEmpty()
                .MaximumLength(maximumLength)
                .Must(s => char.IsUpper(s.FirstOrDefault()));

            RuleFor(user => user.LastName)
                .NotEmpty()
                .MaximumLength(maximumLength)
                .Must(s => char.IsUpper(s.FirstOrDefault()));

            RuleFor(user => user.UserName)
                .NotEmpty()
                .Length(minimumLength, maximumLength);

            RuleFor(user => user.Region)
                .NotEmpty()
                .MaximumLength(maximumLength);
        }
    }
}
