using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Analytics
{
    public class MonthlyProgressDto
    {
        public string Month { get; set; } = string.Empty;

        public int TotalPractice { get; set; }

        public int TotalContests { get; set; }

        public double AverageScore { get; set; }
    }
}
