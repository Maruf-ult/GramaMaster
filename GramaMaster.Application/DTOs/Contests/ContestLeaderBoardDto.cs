using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Contests
{
    public class ContestLeaderBoardDto
    {
        public int Rank { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public double Score { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public TimeSpan CompletionTime { get; set; }
    }
}
