using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Teacher
{
    public class TeacherAnalyticsDto
    {
        public int TotalStudents { get; set; }

        public int TotalProblemsCreated { get; set; }

        public int AiGeneratedProblems { get; set; }

        public int ManualProblems { get; set; }

        public int TotalContests { get; set; }

        public int RunningContests { get; set; }

        public int CompletedContests { get; set; }

        public double AverageContestScore { get; set; }

        public double AverageStudentAccuracy { get; set; }

        public List<TopicPerformanceDto> TopicPerformances { get; set; } = new();
    }
}
