using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Domain.Entities
{
    public class GrammarRule:BaseEntity
    {

        public Guid TopicId { get; set; }
        public Topic Topic { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Example { get; set; } = null!;

    }
}
