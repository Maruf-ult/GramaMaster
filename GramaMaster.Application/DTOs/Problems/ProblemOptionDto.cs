using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Problems
{
    public class ProblemOptionDto
    {
        public Guid Id { get; set; }

        public string OptionText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }
}
