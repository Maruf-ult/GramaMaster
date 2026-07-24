using GramaMaster.Domain.Entities;
using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Contests
{
    public class CreateContestDto
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Guid CurriculumId { get; set; }

        public ContestType ContestType { get; set; }

        public Guid? TeamId { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public int DurationMinutes { get; set; }

        public List<Guid> ProblemIds { get; set; } = new();
    }
}
