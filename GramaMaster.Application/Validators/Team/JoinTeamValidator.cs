using FluentValidation;
using GramaMaster.Application.DTOs.Team;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Validators.Team
{
    public class JoinTeamValidator:AbstractValidator<JoinTeamDto>
    {
        public JoinTeamValidator()
        {
            RuleFor(x => x.JoinCode)
                .NotEmpty().WithMessage("Join code is required.")
                .Length(6).WithMessage("Join code must be exactly 6 characters long.");

            RuleFor(x => x.JoinCode)
                .Matches("^[A-Z0-9]+$")
                .WithMessage("Invalid join code.");

        }
    }
}
