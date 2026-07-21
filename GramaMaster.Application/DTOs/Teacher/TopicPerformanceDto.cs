using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Teacher
{
    public class TopicPerformanceDto
    {
        public Guid TopicId { get; set; }

        public string TopicName { get; set; } = string.Empty;

        public int QuestionsSolved { get; set; }

        public double AverageAccuracy { get; set; }

        public int WeakStudents { get; set; }

        public int StrongStudents { get; set; }
    }
}
