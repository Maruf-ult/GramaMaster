using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Teacher
{
    public class TeamSummaryDto
    {
        public Guid TeamId { get; set; }

        public string TeamName { get; set; } = string.Empty;

        public int StudentCount { get; set; }
    }
}
