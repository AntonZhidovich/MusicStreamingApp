using FluentValidation;
using MusicService.Application.Models.AuthorService;

namespace MusicService.Application.Validators
{
    public class UpdateAuthorValidator : AbstractValidator<UpdateAuthorRequest>
    {
        private const int descriptionMaxLength = 300;
        private readonly DateTime minimumDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public UpdateAuthorValidator()
        {
            RuleFor(request => request.Description)
                .MaximumLength(descriptionMaxLength);

            RuleFor(request => request.BrokenAt)
                .GreaterThan(minimumDate)
                .LessThan(DateTime.UtcNow);
        }
    }
}
