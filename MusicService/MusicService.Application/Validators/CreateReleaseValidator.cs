using FluentValidation;
using MusicService.Application.Models.ReleaseService;

namespace MusicService.Application.Validators
{
    public class CreateReleaseValidator : AbstractValidator<CreateReleaseRequest>
    {
        private const int nameMaxLength = 100;
        private const int authorNameMaxLength = 50;
        private readonly DateTime minimumDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public CreateReleaseValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                .MaximumLength(nameMaxLength);

            RuleFor(request => request.ReleasedAt)
                .GreaterThan(minimumDate)
                .LessThan(DateTime.UtcNow);

            RuleFor(request => request.AuthorNames)
                .NotEmpty();

            RuleForEach(request => request.AuthorNames)
                .NotEmpty()
                .MaximumLength(authorNameMaxLength);

            RuleFor(request => request.Songs)
                .NotEmpty()
                .ForEach(rule => rule.SetValidator(new AddSongToReleaseValidator()));
                
        }
    }
}
