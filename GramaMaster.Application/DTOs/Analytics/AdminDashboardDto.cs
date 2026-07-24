using GramaMaster.Application.DTOs.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Analytics
{
    public class AdminDashboardDto
    {
        public int TotalStudents { get; set; }

        public int TotalTeachers { get; set; }

        public int TotalContests { get; set; }

        public int TotalProblems { get; set; }

        public int TotalPracticeSessions { get; set; }

        public int TotalAIRequests { get; set; }

        public double TotalAICost { get; set; }

        public List<MonthlyProgressDto> MonthlyStatistics { get; set; } = new();
        public List<AIUsageDto> AIUsage { get; set; } = new();
    }
}
