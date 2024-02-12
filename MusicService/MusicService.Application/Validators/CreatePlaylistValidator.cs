using FluentValidation;
using MusicService.Application.Models.PlaylistService;

namespace MusicService.Application.Validators
{
    public class CreatePlaylistValidator : AbstractValidator<CreatePlaylistRequest>
    {
        private const int nameMaxLength = 50;

        public CreatePlaylistValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                .MaximumLength(nameMaxLength);
        }
    }
}
