using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.GrammerRules
{
    public class CreateGrammarRuleDto
    {
        public Guid TopicId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Example { get; set; } = string.Empty;
    }
}
