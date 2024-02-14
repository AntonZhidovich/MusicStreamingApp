using FluentValidation;
using MusicService.Application.Models.SongService;
using System.Globalization;

namespace MusicService.Application.Validators
{
    public class UpdateSongValidator : AbstractValidator<UpdateSongRequest>
    {
        private const int titleMaxLength = 50;
        private const int sourceMaxLength = 100;

        public UpdateSongValidator()
        {
            RuleFor(request => request.Title).MaximumLength(titleMaxLength);
            RuleFor(request => request.SourceName).MaximumLength(sourceMaxLength);
        }
    }
}
