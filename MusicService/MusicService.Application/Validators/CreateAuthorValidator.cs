using FluentValidation;
using MusicService.Application.Models.AuthorService;

namespace MusicService.Application.Validators
{
    public class CreateAuthorValidator : AbstractValidator<CreateAuthorRequest>
    {
        private const int nameMaxLength = 50;
        private const int descriptionMaxLength = 300;
        private readonly DateTime minimumDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public CreateAuthorValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                .MaximumLength(nameMaxLength);

            RuleFor(request => request.Description)
                .MaximumLength(descriptionMaxLength);

            RuleFor(request => request.CreatedAt)
                .NotEmpty()
                .GreaterThan(minimumDate)
                .LessThan(DateTime.UtcNow);

            RuleFor(request => request.UserNames).NotEmpty();
        }
    }
}
