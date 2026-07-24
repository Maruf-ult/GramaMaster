using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.AI
{
    public class GenerateQuestionsDto
    {
        public Guid TopicId { get; set; }

        public Guid CurriculumId { get; set; }

        public ProblemType ProblemType { get; set; }

        public DifficultyType Difficulty { get; set; }

        public int Count { get; set; }

        public bool SaveToDatabase { get; set; } = true;
    }
}
