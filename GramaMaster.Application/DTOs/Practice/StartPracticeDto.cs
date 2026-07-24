using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Practice
{
    public class StartPracticeDto
    {
        public Guid TopicId { get; set; }

        public Guid CurriculumId { get; set; }

        public DifficultyType Difficulty { get; set; }

        public int QuestionCount { get; set; } = 10;
    }
}
