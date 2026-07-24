using FluentValidation;
using GramaMaster.Application.DTOs.Exams;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Validators.Exams
{
    public class SubmitExamValidator
    : AbstractValidator<SubmitExamDto>
    {
        public SubmitExamValidator()
        {
            RuleFor(x => x.AttemptId)
                .NotEmpty().WithMessage("Attempt ID is required.");

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
