using FluentValidation;
using GramaMaster.Application.DTOs.Problems;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Validators.Problems
{
    public class UpdateProblemValidator : AbstractValidator<UpdateProblemDto>
    {
        public UpdateProblemValidator()
        {
            RuleFor(x => x.TopicId)
                .NotEmpty().WithMessage("Topic ID is required.");

            RuleFor(x => x.QuestionText)
                .NotEmpty().WithMessage("Question text is required.")
                .MaximumLength(3000).WithMessage("Question text must not exceed 3000 characters.");

            RuleFor(x => x.CorrectAns)
                .NotEmpty().WithMessage("Correct answer is required.")
                .MaximumLength(1000).WithMessage("Correct answer must not exceed 1000 characters.");

            RuleFor(x => x.Explanation)
                .NotEmpty().WithMessage("Explanation is required.")
                .MaximumLength(5000).WithMessage("Explanation must not exceed 5000 characters.");

            RuleFor(x => x.Options)
                .Must(x => x.Count == 4)
                .When(x => x.ProblemType == Domain.Enums.ProblemType.MCQ)
                .WithMessage("MCQ must contain exactly 4 options.");
        }
    }
}
