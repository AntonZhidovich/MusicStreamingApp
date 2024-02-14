using FluentValidation;
using MusicService.Application.Models.ReleaseService;
using System.Globalization;

namespace MusicService.Application.Validators
{
    public class AddSongToReleaseValidator : AbstractValidator<AddSongToReleaseRequest>
    {
        private const int titleMaxLength = 50;
        private const int sourceMaxLength = 100;
        private const string timeSpanFormat = "hh\\:mm\\:ss";
        private const int genreMaxLength = 30;

        public AddSongToReleaseValidator()
        {
            RuleFor(request => request.Title)
                .NotEmpty()
                .MaximumLength(titleMaxLength);

            TimeSpan bufTimeSpan;
            RuleFor(request => request.DurationMinutes)
                .NotEmpty()
                .Must(source => TimeSpan.TryParseExact(source, timeSpanFormat, CultureInfo.InvariantCulture, out bufTimeSpan))
                .WithMessage("DurationMinutes doesn't represent Time Span.")
                .MaximumLength(timeSpanFormat.Length);

            RuleFor(request => request.Genres).NotEmpty();
            RuleForEach(request => request.Genres).NotEmpty().MaximumLength(genreMaxLength);
            RuleFor(request => request.SourceName).NotEmpty().MaximumLength(sourceMaxLength);
        }
    }
}
