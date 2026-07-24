using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.AI
{
    public class AIQuestionDto
    {
        public string QuestionText { get; set; } = string.Empty;

        public List<string> Options { get; set; } = new();

        public string CorrectAnswer { get; set; } = string.Empty;

        public string Explanation { get; set; } = string.Empty;

        public DifficultyType Difficulty { get; set; }

        public ProblemType ProblemType { get; set; }
    }
}
