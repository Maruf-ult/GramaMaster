using GramaMaster.Application.DTOs.Exams;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Practice
{
    public class PracticeResultDto
    {
        public int TotalQuestions { get; set; }

        public int CorrectAnswers { get; set; }

        public double ScorePercentage { get; set; }

        public List<AnswerReviewDto> Reviews { get; set; } = new();
    }
}
