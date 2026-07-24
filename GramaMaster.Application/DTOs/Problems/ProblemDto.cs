using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Problems
{
    public class ProblemDto
    {
        public Guid Id { get; set; }

        public string Topic { get; set; } = string.Empty;

        public ProblemType ProblemType { get; set; }

        public DifficultyType Difficulty { get; set; }

        public string QuestionText { get; set; } = string.Empty;

        public bool IsAiGenerated { get; set; }
    }
}
