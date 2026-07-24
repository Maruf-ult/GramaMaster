using FluentValidation;
using GramaMaster.Application.DTOs.Contests;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Validators.Contests
{
    public class UpdateContestValidator:AbstractValidator<UpdateContestDto>
    {
        public UpdateContestValidator()
        {
            RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");

            RuleFor(x => x.DurationMinutes)
                .InclusiveBetween(10, 300).WithMessage("Duration must be between 10 and 300 minutes.");

            RuleFor(x => x.EndAt)
                .GreaterThan(x => x.StartAt).WithMessage("End time must be after start time.");

            RuleFor(x => x.ProblemIds)
                .NotEmpty().WithMessage("Problem IDs are required.");
        }
    }
}
