using FluentValidation;
using MusicService.Application.Models.SongService;

namespace MusicService.Application.Validators
{
    public class ChangeGenreDescriptionValidator : AbstractValidator<UpdateGenreRequest>
    {
        private const int descriptionMaxLength = 300;

        public ChangeGenreDescriptionValidator()
        {
            RuleFor(request => request.NewDescription)
                .NotEmpty()
                .MaximumLength(descriptionMaxLength);
        }
    }
}
