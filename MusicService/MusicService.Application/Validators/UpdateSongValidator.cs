using FluentValidation;
using MusicService.Application.Models.SongService;
using MusicService.Domain.Constants;

namespace MusicService.Application.Validators
{
    public class UpdateSongValidator : AbstractValidator<UpdateSongRequest>
    {
        public UpdateSongValidator()
        {
            RuleFor(request => request.Title).MaximumLength(Constraints.songTitleMaxLength);
            RuleFor(request => request.SourceName).MaximumLength(Constraints.songSourceMaxLength);
        }
    }
}
