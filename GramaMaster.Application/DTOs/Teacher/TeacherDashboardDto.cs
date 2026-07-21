using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Teacher
{
    public class TeacherDashboardDto
    {
        public int TotalTeams { get; set; }

        public int TotalStudents { get; set; }

        public int TotalContests { get; set; }

        public int TotalPracticeProblems { get; set; }

        public int AiGeneratedProblems { get; set; }

        public int ActiveContests { get; set; }

        public double AverageStudentAccuracy { get; set; }

        public List<RecentContestDto> RecentContests { get; set; } = new();

        public List<TeamSummaryDto> Teams { get; set; } = new();
    }
}
