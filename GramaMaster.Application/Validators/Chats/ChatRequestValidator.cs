using FluentValidation;
using GramaMaster.Application.DTOs.Chats;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Validators.Chats
{
    public class ChatRequestValidator
     : AbstractValidator<ChatRequestDto>
    {
        public ChatRequestValidator()
        {
            RuleFor(x => x.SessionId)
                .NotEmpty().WithMessage("Session ID is required.");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required.")
                .MaximumLength(2000).WithMessage("Message cannot exceed 2000 characters.");
        }
    }
}
