using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Domain.Entities
{
    public class ProblemOptions:BaseEntity
    {
        public Guid ProblemId { get; set; }
        public Problem Problem { get; set; } = null!;
        public string? OptionText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
