using FluentValidation;
using GramaMaster.Application.DTOs.GrammerRules;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Validators.GrammerRules
{
    public class UpdateGrammarRuleValidator:AbstractValidator<UpdateGrammarRuleDto>
    {
        public UpdateGrammarRuleValidator()
        {
            RuleFor(x => x.Title)
               .NotEmpty().WithMessage("Title is required.")
               .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");
           
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(5000).WithMessage("Content must not exceed 5000 characters.");
            
            RuleFor(x => x.Example)
                .NotEmpty().WithMessage("Example is required.")
                .MaximumLength(2000).WithMessage("Example must not exceed 2000 characters.");
        }
    }
}
