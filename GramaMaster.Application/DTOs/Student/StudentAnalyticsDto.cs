using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Student
{
    public class StudentAnalyticsDto
    {
        public int TotalQuestionsSolved { get; set; }

        public int CorrectAnswers { get; set; }

        public int WrongAnswers { get; set; }

        public double Accuracy { get; set; }

        public TimeSpan AverageTimePerQuestion { get; set; }
    }
}
