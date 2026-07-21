using FluentValidation;
using GramaMaster.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Validators.Auth
{
    public class ForgotPasswordValidator:AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        }
    }
}
