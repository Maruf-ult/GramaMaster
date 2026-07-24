using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Analytics
{
    public class StudentDashboardDto
    {
        public string StudentName { get; set; } = string.Empty;

        public int PracticeCompleted { get; set; }

        public int ContestsJoined { get; set; }

        public double AverageScore { get; set; }

        public int WeakTopics { get; set; }

        public int StrongTopics { get; set; }

        public List<TopicStatisticsDto> TopicStatistics { get; set; } = new();

        public List<PerformanceTrendDto> Performance { get; set; } = new();

        public List<MonthlyProgressDto> MonthlyProgress { get; set; } = new();
    }
}
