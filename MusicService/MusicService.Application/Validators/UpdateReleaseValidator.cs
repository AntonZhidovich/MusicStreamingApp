using FluentValidation;
using MusicService.Application.Models.ReleaseService;
using MusicService.Domain.Constants;

namespace MusicService.Application.Validators
{
    public class UpdateReleaseValidator : AbstractValidator<UpdateReleaseRequest>
    {
        public UpdateReleaseValidator()
        {
            RuleFor(request => request.Name)
                .MaximumLength(Constraints.releaseNameMaxLength);

            RuleFor(request => request.ReleasedAt)
                .GreaterThan(Constraints.minimumDate)
                .LessThan(DateTime.UtcNow);
        }
    }
}
