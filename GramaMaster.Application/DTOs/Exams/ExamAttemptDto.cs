using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Exams
{
    public class ExamAttemptDto
    {
        public Guid Id { get; set; }

        public Guid ContestId { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime? SubmittedAt { get; set; }

        public double Score { get; set; }

        public int TotalQuestions { get; set; }
    }
}
