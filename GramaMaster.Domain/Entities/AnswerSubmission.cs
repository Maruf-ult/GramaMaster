using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Domain.Entities
{
    public class AnswerSubmission:BaseEntity
    {
        public Guid ExamAttemptId { get; set; }
        public ExamAttempt ExamAttempt { get; set; } = null!;
        public Guid ProblemId { get; set; }
        public Problem Problem { get; set; } = null!;
        public string StudentAnswer { get; set; } = null!;
        public bool IsCorrect { get; set; }
        public string? AIExplanation { get; set; }
    }
}
