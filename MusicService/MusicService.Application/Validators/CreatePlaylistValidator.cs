using FluentValidation;
using MusicService.Application.Models.PlaylistService;
using MusicService.Domain.Constants;

namespace MusicService.Application.Validators
{
    public class CreatePlaylistValidator : AbstractValidator<CreatePlaylistRequest>
    {
        public CreatePlaylistValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                .MaximumLength(Constraints.playlistNameMaxLength);
        }
    }
}
