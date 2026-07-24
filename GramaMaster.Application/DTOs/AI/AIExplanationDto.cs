using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.AI
{
    public class AIExplanationDto
    {
        public Guid ProblemId { get; set; }

        public string StudentAnswer { get; set; } = string.Empty;

        public string CorrectAnswer { get; set; } = string.Empty;

        public string Explanation { get; set; } = string.Empty;
    }
}
