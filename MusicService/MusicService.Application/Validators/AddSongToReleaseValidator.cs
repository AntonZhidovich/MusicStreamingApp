using FluentValidation;
using MusicService.Application.Models.ReleaseService;
using MusicService.Domain.Constants;
using System.Globalization;

namespace MusicService.Application.Validators
{
    public class AddSongToReleaseValidator : AbstractValidator<AddSongToReleaseRequest>
    {
        public AddSongToReleaseValidator()
        {
            RuleFor(request => request.Title)
                .NotEmpty()
                .MaximumLength(Constraints.songTitleMaxLength);

            TimeSpan bufTimeSpan;
            RuleFor(request => request.DurationMinutes)
                .NotEmpty()
                .Must(source => TimeSpan.TryParseExact(source, Constraints.timeSpanFormat, CultureInfo.InvariantCulture, out bufTimeSpan))
                .WithMessage("DurationMinutes doesn't represent Time Span.")
                .MaximumLength(Constraints.timeSpanFormat.Length);

            RuleFor(request => request.Genres).NotEmpty();
            RuleForEach(request => request.Genres).NotEmpty().MaximumLength(Constraints.genreNameMaxLength);
            RuleFor(request => request.SourceName).NotEmpty().MaximumLength(Constraints.songSourceMaxLength);
        }
    }
}
