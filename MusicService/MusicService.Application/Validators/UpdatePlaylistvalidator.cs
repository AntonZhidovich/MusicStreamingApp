using FluentValidation;
using MusicService.Application.Models.PlaylistService;
using MusicService.Domain.Constants;

namespace MusicService.Application.Validators
{
    public class UpdatePlaylistvalidator : AbstractValidator<UpdatePlaylistRequest>
    {
        public UpdatePlaylistvalidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                .MaximumLength(Constraints.playlistNameMaxLength);
        }
    }
}
