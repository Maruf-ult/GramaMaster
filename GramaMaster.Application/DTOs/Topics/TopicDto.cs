using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Topics
{
    public class TopicDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid CurriculumId { get; set; }
        public string Curriculum { get; set; } = string.Empty;
        public int ProblemCount { get; set; }
        public int GrammarRuleCount { get; set; }
    }
}
