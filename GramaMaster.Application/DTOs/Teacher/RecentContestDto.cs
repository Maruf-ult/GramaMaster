using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Teacher
{
    public class RecentContestDto
    {
        public Guid ContestId { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public int ParticipantCount { get; set; }
    }
}
