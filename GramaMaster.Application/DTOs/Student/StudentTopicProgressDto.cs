using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Student
{
    public class StudentTopicProgressDto
    {
        public Guid TopicId { get; set; }

        public string TopicName { get; set; } = string.Empty;

        public int TotalSolved { get; set; }

        public int CorrectAnswers { get; set; }

        public int WrongAnswers { get; set; }

        public double Accuracy { get; set; }

        public double ProgressPercentage { get; set; }

        public string DifficultyRecommendation { get; set; } = string.Empty;
    }
}
