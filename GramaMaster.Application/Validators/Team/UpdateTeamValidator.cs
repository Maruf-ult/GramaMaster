using FluentValidation;
using GramaMaster.Application.DTOs.Team;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Validators.Team
{
    public class UpdateTeamValidator:AbstractValidator<UpdateTeamDto>
    {
        public UpdateTeamValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Team name is required.")
               .MaximumLength(150).WithMessage("Team name must not exceed 150 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Team description is required.")
                .MaximumLength(1000).WithMessage("Team description must not exceed 1000 characters.");
        }
    }
}
