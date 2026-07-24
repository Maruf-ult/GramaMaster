using FluentValidation;
using GramaMaster.Application.DTOs.Topics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Validators.Topic
{
    public class CreateTopicValidator:AbstractValidator<CreateTopicDto>
    {
        public CreateTopicValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Topic name is required.")
                .MaximumLength(100).WithMessage("Topic name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.");
            
            RuleFor(x => x.CurriculumId)
            .NotEmpty().WithMessage("CurriculumId is required.");
        }
    }
}
