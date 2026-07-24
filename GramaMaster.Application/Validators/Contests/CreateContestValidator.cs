using FluentValidation;
using GramaMaster.Application.DTOs.Contests;
using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Validators.Contests
{
    public class CreateContestValidator:AbstractValidator<CreateContestDto>
    {
        public CreateContestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Contest title is required.")
                .MaximumLength(150).WithMessage("Contest title must not exceed 150 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Contest description is required.")
                .MaximumLength(2000).WithMessage("Contest description must not exceed 2000 characters.");

            RuleFor(x => x.CurriculumId)
                .NotEmpty().WithMessage("Curriculum ID is required.");

            RuleFor(x => x.DurationMinutes)
            .InclusiveBetween(10, 300).WithMessage("Contest duration must be between 10 and 300 minutes.");

            RuleFor(x => x.StartAt)
                .GreaterThan(DateTime.UtcNow).WithMessage("Contest start time must be in the future.");

            RuleFor(x => x.EndAt)
                .GreaterThan(x => x.StartAt).WithMessage("Contest end time must be after the start time.");

            RuleFor(x => x.ProblemIds)
                .NotEmpty().WithMessage("Problem IDs are required.");

            RuleFor(x => x.TeamId)
                .NotNull()
                .When(x => x.ContestType == ContestType.Local)
                .WithMessage("Local contests must belong to a team.");
        }
    }
}
