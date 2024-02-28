using FluentValidation;
using MediatR;
using SubscriptionService.BusinessLogic.Constants;
using SubscriptionService.BusinessLogic.Exceptions;
using SubscriptionService.BusinessLogic.Models;

namespace SubscriptionService.BusinessLogic.Features.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseRequest
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var validationErrors = _validators
                .Select(validator => validator.Validate(request))
                .SelectMany(result => result.Errors)
                .GroupBy(error => error.PropertyName,
                error => error.ErrorMessage,
                (propertyName, errorMessages) =>
                new ErrorDetail(propertyName, errorMessages))
                .ToList();

            if (validationErrors.Any())
            {
                throw new ValidationPipelineException(ExceptionMessages.validationError, validationErrors);
            }

            return await next();
        }
    }
}
