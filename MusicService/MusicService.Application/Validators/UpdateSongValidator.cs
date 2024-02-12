using FluentValidation;
using MusicService.Application.Models.SongService;

namespace MusicService.Application.Validators
{
    public class UpdateSongValidator : AbstractValidator<UpdateSongRequest>
    {
        private const int titleMaxLength = 50;
        private const int sourceMaxLength = 100;
        private const int maxDuration = 60;

        public UpdateSongValidator()
        {
            RuleFor(request => request.Title).MaximumLength(titleMaxLength);
            RuleFor(request => request.SourceName).MaximumLength(sourceMaxLength);
            
            RuleFor(request => request.DurationMinutes)
                .GreaterThan(0)
                .LessThan(maxDuration);
        }
    }
}
