using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Domain.Entities
{
    public class Topic:BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public Guid CurriculumId { get; set; }
        public Curriculum Curriculum { get; set; } = null!;

        public ICollection<Problem> Problems { get; set;} = new List<Problem>();
        public ICollection<GrammarRule> GrammerRules { get; set; } = new List<GrammarRule>();
    }
}
