﻿using Microsoft.AspNetCore.Identity;

namespace Identity.BusinessLogic.Exceptions
{
    public class InvalidAuthorizationException : Exception
    {
        public IEnumerable<IdentityError>? Errors { get; set; }

        public InvalidAuthorizationException(string message, IEnumerable<IdentityError>? errors = null) 
            : base(message)
        {
            Errors = errors;
        }
    }
}
