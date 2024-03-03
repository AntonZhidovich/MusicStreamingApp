using FluentValidation;
using Identity.BusinessLogic.Models;

namespace Identity.BusinessLogic.Validators
{
    public class TokensValidator : AbstractValidator<Tokens>
    {
        public TokensValidator()
        {
            RuleFor(tokens => tokens.AccessToken).NotEmpty();
            RuleFor(tokens => tokens.RefreshToken).NotEmpty();
        }
    }
}
