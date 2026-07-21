using FluentValidation;
using GramaMaster.Application.DTOs.Teacher;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Validators.Teacher
{
    public class UpdateTeacherProfileValidator:AbstractValidator<UpdateTeacherProfileDto>
    {
        public UpdateTeacherProfileValidator()
        {
            RuleFor(x => x.Department)
                .NotEmpty().WithMessage("Department is required.")
                .MaximumLength(200).WithMessage("Department name is too long.");

            RuleFor(x => x.University)
                .NotEmpty().WithMessage("University is required")
                .MaximumLength(200).WithMessage("University name is too long");

            RuleFor(x => x.ProfileImageUrl)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.ProfileImageUrl));
        }
    }
}
