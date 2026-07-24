using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Exams
{
    public class LeaderboardDto
    {
        public int Rank { get; set; }

        public string StudentName { get; set; } = string.Empty;

        public double Score { get; set; }

        public DateTime SubmittedAt { get; set; }
    }
}
