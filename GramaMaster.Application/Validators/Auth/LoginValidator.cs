using FluentValidation;
using GramaMaster.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Validators.Auth
{
    public class LoginValidator:AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
