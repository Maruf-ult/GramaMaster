using FluentValidation;
using GramaMaster.Application.DTOs.Problems;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Validators.Problems
{
    public class GenerateAIProblemValidator : AbstractValidator<GenerateAiProblemsDto>
    {
        public GenerateAIProblemValidator()
        {
            RuleFor(x => x.TopicId)
                .NotEmpty().WithMessage("Topic ID is required.");

            RuleFor(x => x.Count)
                .InclusiveBetween(1, 50).WithMessage("Count must be between 1 and 50.");
        }
    }
}
