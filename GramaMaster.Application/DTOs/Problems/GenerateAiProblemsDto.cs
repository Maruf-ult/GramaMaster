using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Problems
{
    public class GenerateAiProblemsDto
    {
        public Guid TopicId { get; set; }

        public ProblemType ProblemType { get; set; }

        public DifficultyType Difficulty { get; set; }

        public int Count { get; set; } = 10;
    }
}
