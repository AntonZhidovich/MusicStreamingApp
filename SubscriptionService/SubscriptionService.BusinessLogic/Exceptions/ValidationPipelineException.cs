using SubscriptionService.BusinessLogic.Models;

namespace SubscriptionService.BusinessLogic.Exceptions
{
    public class ValidationPipelineException : Exception
    {
        public IEnumerable<ErrorDetail> ValidationErrors { get; set; }

        public ValidationPipelineException(string message,
            IEnumerable<ErrorDetail> validationErrors) : base(message) 
        {
            ValidationErrors = validationErrors;
        }
    }
}
