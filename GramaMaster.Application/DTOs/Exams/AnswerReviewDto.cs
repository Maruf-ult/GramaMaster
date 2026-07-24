using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Exams
{
    public class AnswerReviewDto
    {
        public Guid ProblemId { get; set; }

        public string Question { get; set; } = string.Empty;

        public string StudentAnswer { get; set; } = string.Empty;

        public string CorrectAnswer { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }

        public string? AIExplanation { get; set; }
    }
}
