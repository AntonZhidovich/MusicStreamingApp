using FluentValidation;
using MusicService.Application.Models.ReleaseService;
using MusicService.Domain.Constants;

namespace MusicService.Application.Validators
{
    public class CreateReleaseValidator : AbstractValidator<CreateReleaseRequest>
    {
        public CreateReleaseValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                .MaximumLength(Constraints.releaseNameMaxLength);

            RuleFor(request => request.ReleasedAt)
                .GreaterThan(Constraints.minimumDate)
                .LessThan(DateTime.UtcNow);

            RuleFor(request => request.AuthorNames)
                .NotEmpty();

            RuleForEach(request => request.AuthorNames)
                .NotEmpty()
                .MaximumLength(Constraints.authorNameMaxLength);

            RuleFor(request => request.Songs)
                .NotEmpty()
                .ForEach(rule => rule.SetValidator(new AddSongToReleaseValidator()));      
        }
    }
}
