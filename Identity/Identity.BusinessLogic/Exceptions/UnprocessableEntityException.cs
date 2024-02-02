using Microsoft.AspNetCore.Identity;

namespace Identity.BusinessLogic.Exceptions
{
    public class UnprocessableEntityException : BaseIdentityException
    {
        public UnprocessableEntityException(string message, IEnumerable<IdentityError>? errors = null) : base(message, errors) 
        {
        }
    }
}
