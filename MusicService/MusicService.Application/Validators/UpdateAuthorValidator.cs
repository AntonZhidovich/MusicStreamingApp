using FluentValidation;
using MusicService.Application.Models.AuthorService;
using MusicService.Domain.Constants;

namespace MusicService.Application.Validators
{
    public class UpdateAuthorValidator : AbstractValidator<UpdateAuthorRequest>
    {
        public UpdateAuthorValidator()
        {
            RuleFor(request => request.Description)
                .MaximumLength(Constraints.descriptionMaxLength);

            RuleFor(request => request.BrokenAt)
                .GreaterThan(Constraints.minimumDate)
                .LessThan(DateTime.UtcNow);
        }
    }
}
