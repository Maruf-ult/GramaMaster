using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Problems
{
    public class CreateProblemDto
    {
        public Guid TopicId { get; set; }

        public Guid? ContestId { get; set; }

        public ProblemType ProblemType { get; set; }

        public DifficultyType Difficulty { get; set; }

        public string QuestionText { get; set; } = string.Empty;

        public string CorrectAns { get; set; } = string.Empty;

        public string Explanation { get; set; } = string.Empty;

        public List<ProblemOptionDto> Options { get; set; } = new();
    }
}
