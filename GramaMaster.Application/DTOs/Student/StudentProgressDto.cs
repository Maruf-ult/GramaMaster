using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Student
{
    public class StudentProgressDto
    {
        public int TotalTopics { get; set; }

        public int CompletedTopics { get; set; }

        public double ProgressPercentage { get; set; }

        public List<StudentTopicProgressDto> Topics { get; set; } = new();
    }
}
