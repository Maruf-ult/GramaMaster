using FluentValidation;
using GramaMaster.Application.DTOs.Student;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Validators.Student
{
    public class UpdateStudentProfileValidator:AbstractValidator<StudentProfileDto>
    {
        public UpdateStudentProfileValidator()
        {
            RuleFor(x => x.InstitutionName)
                .NotEmpty().WithMessage("Institution name is required.")
                .MaximumLength(200).WithMessage("Institution name cannot exceed 200 characters.");
                
            RuleFor(x => x.ProfileImageUrl)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.ProfileImageUrl));
        }
    }
}
