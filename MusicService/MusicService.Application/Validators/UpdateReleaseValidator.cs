using FluentValidation;
using MusicService.Application.Models.ReleaseService;

namespace MusicService.Application.Validators
{
    public class UpdateReleaseValidator : AbstractValidator<UpdateReleaseRequest>
    {
        private const int nameMaxLength = 100;
        private readonly DateTime minimumDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public UpdateReleaseValidator()
        {
            RuleFor(request => request.Name).MaximumLength(nameMaxLength);

            RuleFor(request => request.ReleasedAt)
                .GreaterThan(minimumDate)
                .LessThan(DateTime.UtcNow);
        }
    }
}
