using FluentValidation;
using GramaMaster.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Validators.Auth
{
    public class RegisterTeacherValidator:AbstractValidator<RegisterTeacherDto>
    {
        public RegisterTeacherValidator()
        {
            RuleFor(x => x.FullName)
               .NotEmpty().WithMessage("Full name is required.")
               .MaximumLength(200).WithMessage("Full name cannot exceed 200 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.Department)
                .NotEmpty().WithMessage("Department is required.")
                .MaximumLength(400).WithMessage("Department cannot exceed 400 characters.");
            
            RuleFor(x => x.University)
                .NotEmpty().WithMessage("University is required.")
                .MaximumLength(400).WithMessage("University cannot exceed 400 characters.");

            RuleFor(x => x.SscResult)
                .NotEmpty().WithMessage("SSC Result is required.")
                .InclusiveBetween(0, 5).WithMessage("SSC Result must be between 0 and 5.");
            
            RuleFor(x => x.HscResult)
                .NotEmpty().WithMessage("HSC Result is required.")
                .InclusiveBetween(0,5).WithMessage("HSC Result must be between 0 and 5.");

        }
    }
}
