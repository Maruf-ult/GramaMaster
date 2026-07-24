using FluentValidation;
using GramaMaster.Application.DTOs.Practice;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Validators.Practice
{
    public class PracticeSubmissionValidator
    : AbstractValidator<PracticeSubmissionDto>
    {
        public PracticeSubmissionValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("Student ID is required.");

            RuleFor(x => x.Answers)
                .NotEmpty().WithMessage("Answers are required.");

            RuleForEach(x => x.Answers)
                .ChildRules(answer =>
                {
                    answer.RuleFor(x => x.ProblemId)
                        .NotEmpty().WithMessage("Problem ID is required.");

                    answer.RuleFor(x => x.StudentAnswer)
                        .NotEmpty().WithMessage("Student answer is required.");
                });
        }
    }
}
