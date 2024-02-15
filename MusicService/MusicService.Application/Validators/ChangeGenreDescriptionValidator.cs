using FluentValidation;
using MusicService.Application.Models.SongService;
using MusicService.Domain.Constants;

namespace MusicService.Application.Validators
{
    public class ChangeGenreDescriptionValidator : AbstractValidator<UpdateGenreRequest>
    {
        public ChangeGenreDescriptionValidator()
        {
            RuleFor(request => request.NewDescription)
                .NotEmpty()
                .MaximumLength(Constraints.descriptionMaxLength);
        }
    }
}
