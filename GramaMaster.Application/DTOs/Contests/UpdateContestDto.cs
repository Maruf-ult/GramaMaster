using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Contests
{
    public class UpdateContestDto
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public int DurationMinutes { get; set; }

        public List<Guid> ProblemIds { get; set; } = new();
    }
}
