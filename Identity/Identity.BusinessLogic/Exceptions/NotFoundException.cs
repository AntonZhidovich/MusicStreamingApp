using Microsoft.AspNetCore.Identity;

namespace Identity.BusinessLogic.Exceptions
{
    public class NotFoundException : BaseIdentityException
    {
        public NotFoundException(string message, IEnumerable<IdentityError>? errors = null) : base(message, errors) 
        {
        }    
    }
}
