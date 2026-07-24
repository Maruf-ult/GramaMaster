using GramaMaster.Application.DTOs.Problems;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Contests
{
     public class ContestResultDto
    {
        public Guid AttemptId { get; set; }
        public Guid ContestId { get; set; }
        public string ContestTitle { get; set; } = string.Empty;
        public double Score { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public int Rank { get; set; }
        public TimeSpan TimeTaken { get; set; }
        public List<ProblemResultDto> Problems { get; set; } = new();
    }
}
