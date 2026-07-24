using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.GrammerRules
{
    public class GrammarRuleDto
    {
        public Guid Id { get; set; }
        public Guid TopicId { get; set; }
        public string TopicName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Example { get; set; } = string.Empty;
    }
}
