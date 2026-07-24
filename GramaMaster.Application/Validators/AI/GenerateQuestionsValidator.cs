using FluentValidation;
using GramaMaster.Application.DTOs.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Validators.AI
{
    public class GenerateQuestionsValidator
    : AbstractValidator<GenerateQuestionsDto>
    {
        public GenerateQuestionsValidator()
        {
            RuleFor(x => x.TopicId)
                .NotEmpty().WithMessage("Topic ID is required.");

            RuleFor(x => x.CurriculumId)
                .NotEmpty().WithMessage("Curriculum ID is required.");

            RuleFor(x => x.Count)
                .InclusiveBetween(1, 50).WithMessage("Count must be between 1 and 50.");
        }
    }
}
