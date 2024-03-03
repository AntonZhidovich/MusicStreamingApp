using FluentValidation;
using MusicService.Application.Models.AuthorService;
using MusicService.Domain.Constants;

namespace MusicService.Application.Validators
{
    public class CreateAuthorValidator : AbstractValidator<CreateAuthorRequest>
    {
        public CreateAuthorValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                .MaximumLength(Constraints.authorNameMaxLength);

            RuleFor(request => request.Description)
                .MaximumLength(Constraints.descriptionMaxLength);

            RuleFor(request => request.CreatedAt)
                .NotEmpty()
                .GreaterThan(Constraints.minimumDate)
                .LessThan(DateTime.UtcNow);

            RuleFor(request => request.UserNames).NotEmpty();
        }
    }
}
