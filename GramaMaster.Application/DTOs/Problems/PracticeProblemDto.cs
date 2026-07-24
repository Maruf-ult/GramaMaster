using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Problems
{
    internal class PracticeProblemDto
    {
        public Guid Id { get; set; }

        public Guid TopicId { get; set; }

        public string TopicName { get; set; } = string.Empty;

        public ProblemType ProblemType { get; set; }

        public DifficultyType Difficulty { get; set; }

        public string QuestionText { get; set; } = string.Empty;

        public List<ProblemOptionDto> Options { get; set; } = new();
    }
}
