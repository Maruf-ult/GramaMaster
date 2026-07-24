using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Exams
{

    public class ExamResultDto
    {
        public Guid AttemptId { get; set; }

        public double Score { get; set; }

        public int CorrectAnswers { get; set; }

        public int TotalQuestions { get; set; }

        public double Percentage { get; set; }

        public List<AnswerReviewDto> Reviews { get; set; } = new();
    }
}
