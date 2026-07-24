using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Analytics
{
    public class TopicStatisticsDto
    {
        public Guid TopicId { get; set; }

        public string TopicName { get; set; } = string.Empty;

        public int TotalQuestions { get; set; }

        public int CorrectAnswers { get; set; }

        public double Accuracy { get; set; }
    }
}
