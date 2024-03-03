using Microsoft.AspNetCore.Identity;

namespace Identity.BusinessLogic.Exceptions
{
    public abstract class BaseIdentityException : Exception
    {
        public IEnumerable<IdentityError>? Errors { get; set; }

        protected BaseIdentityException(string message, IEnumerable<IdentityError>? errors = null) : base(message) 
        {
            Errors = errors ?? Enumerable.Empty<IdentityError>();
        }
    }
}
