using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Problems
{
    public class ProblemDetailsDto
    {
        public Guid Id { get; set; }

        public Guid TopicId { get; set; }

        public string TopicName { get; set; } = string.Empty;

        public ProblemType ProblemType { get; set; }

        public DifficultyType Difficulty { get; set; }

        public string QuestionText { get; set; } = string.Empty;

        public string CorrectAns { get; set; } = string.Empty;

        public string Explanation { get; set; } = string.Empty;

        public bool IsAiGenerated { get; set; }

        public List<ProblemOptionDto> Options { get; set; } = new();
    }
}
