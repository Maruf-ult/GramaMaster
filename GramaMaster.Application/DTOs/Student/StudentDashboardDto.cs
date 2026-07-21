using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Student
{
    public class StudentDashboardDto
    {
        public int TotalPracticeQuestions { get; set; }

        public int TotalContestsJoined { get; set; }

        public int TotalContestsCompleted { get; set; }

        public double OverallAccuracy { get; set; }

        public int CurrentRank { get; set; }

        public List<StudentTopicProgressDto> WeakTopics { get; set; } = new();

        public List<StudentTopicProgressDto> StrongTopics { get; set; } = new();
    }
}
