using FluentValidation;
using MusicService.Application.Models.PlaylistService;

namespace MusicService.Application.Validators
{
    public class UpdatePlaylistvalidator : AbstractValidator<UpdatePlaylistRequest>
    {
        private const int nameMaxLength = 50;

        public UpdatePlaylistvalidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                .MaximumLength(nameMaxLength);
        }
    }
}
