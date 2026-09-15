using FluentValidation;
using GramaMaster.Application.DTOs.Student;

namespace GramaMaster.Application.Validators.Student
{
    public class UpdateStudentProfileValidator : AbstractValidator<UpdateStudentProfileDto>
    {
        public UpdateStudentProfileValidator()
        {
            RuleFor(x => x.InstitutionName)
                .NotEmpty().WithMessage("Institution name is required.")
                .MaximumLength(300).WithMessage("Institution name cannot exceed 300 characters.");

            RuleFor(x => x.ProfileImageUrl)
                .MaximumLength(1000).WithMessage("Profile image URL cannot exceed 1000 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.ProfileImageUrl));

            RuleFor(x => x.Group)
                .IsInEnum().WithMessage("Invalid study group selected.");

            RuleFor(x => x.Board)
                .IsInEnum().WithMessage("Invalid education board selected.");
        }
    }
}
