using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Domain.Entities
{
    public class ExamAttempt:BaseEntity
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public Guid ContestId { get; set; }
        public Contest Contest { get; set; } = null!;
        public DateTime StartedAt { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public double Score { get; set; }
        public int TotalQuestions { get; set; }
        public ICollection<AnswerSubmission> AnswerSubmissions { get; set; } = new List<AnswerSubmission>();

    }
}
