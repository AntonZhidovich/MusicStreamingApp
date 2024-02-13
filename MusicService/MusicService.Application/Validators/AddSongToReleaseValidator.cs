using FluentValidation;
using MusicService.Application.Models.ReleaseService;

namespace MusicService.Application.Validators
{
    public class AddSongToReleaseValidator : AbstractValidator<AddSongToReleaseRequest>
    {
        private const int titleMaxLength = 50;
        private const int sourceMaxLength = 100;
        private readonly int durationMaxLendth = "hh\\:mm\\:ss".Length;
        private const int genreMaxLength = 30;

        public AddSongToReleaseValidator()
        {
            RuleFor(request => request.Title)
                .NotEmpty()
                .MaximumLength(titleMaxLength);

            RuleFor(request => request.Genres).NotEmpty();
            RuleForEach(request => request.Genres).NotEmpty().MaximumLength(genreMaxLength);
            RuleFor(request => request.DurationMinutes).NotEmpty().MaximumLength(durationMaxLendth);
            RuleFor(request => request.SourceName).NotEmpty().MaximumLength(sourceMaxLength);
        }
    }
}
