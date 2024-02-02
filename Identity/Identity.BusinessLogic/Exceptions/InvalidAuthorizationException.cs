using Microsoft.AspNetCore.Identity;

namespace Identity.BusinessLogic.Exceptions
{
    public class InvalidAuthorizationException : BaseIdentityException
    {
        public InvalidAuthorizationException(string message, IEnumerable<IdentityError>? errors = null) : base(message)
        {
        }
    }
}
