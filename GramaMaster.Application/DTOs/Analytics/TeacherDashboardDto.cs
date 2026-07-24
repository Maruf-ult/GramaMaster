using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Analytics
{
    public class TeacherDashboardDto
    {
        public string TeacherName { get; set; } = string.Empty;

        public int TotalTeams { get; set; }

        public int TotalStudents { get; set; }

        public int TotalContests { get; set; }

        public int TotalProblems { get; set; }

        public double AverageStudentScore { get; set; }

        public List<PerformanceTrendDto> Performance { get; set; } = new();
    }
}
