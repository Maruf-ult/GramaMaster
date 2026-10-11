using FluentValidation;
using GramaMaster.Application.DTOs.Practice;
using System;

namespace GramaMaster.Application.Validators.Practice
{
    public class StartPracticeValidator : AbstractValidator<StartPracticeDto>
    {
        public StartPracticeValidator()
        {
            RuleFor(x => x.TopicId)
                .NotEmpty().WithMessage("Topic ID is required.");

            RuleFor(x => x.CurriculumId)
                .NotEmpty().WithMessage("Curriculum ID is required.");

            RuleFor(x => x.Difficulty)
                .IsInEnum().WithMessage("Valid difficulty type is required.");

            RuleFor(x => x.QuestionCount)
                .GreaterThan(0).WithMessage("Question count must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Question count cannot exceed 100.");
        }
    }
}
