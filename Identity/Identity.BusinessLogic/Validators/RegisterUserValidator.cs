﻿using FluentValidation;
using Identity.BusinessLogic.Constants;
using Identity.BusinessLogic.Models.UserService;

namespace Identity.BusinessLogic.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserValidator()
        {
            RuleFor(user => user.FirstName)
                .NotEmpty()
                .MaximumLength(Constraints.maxNameLength)
                .Must(s => char.IsUpper(s.FirstOrDefault()));

            RuleFor(user => user.LastName)
                .NotEmpty()
                .MaximumLength(Constraints.maxNameLength)
                .Must(s => char.IsUpper(s.FirstOrDefault()));

            RuleFor(user => user.Email)
                .NotEmpty()
                .Length(Constraints.minEmailLength, Constraints.maxEmailLength)
                .EmailAddress();

            RuleFor(user => user.UserName)
                .NotEmpty()
                .Length(Constraints.minUsernameLength, Constraints.maxUsernameLength);

            RuleFor(user => user.Region)
                .NotEmpty()
                .MaximumLength(Constraints.maxRegionLength);
        }
    }
}
