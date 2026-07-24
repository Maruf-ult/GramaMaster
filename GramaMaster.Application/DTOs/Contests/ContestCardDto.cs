using GramaMaster.Domain.Entities;
using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Contests
{
    public class ContestCardDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public ContestType ContestType { get; set; }
        public string Curriculum { get; set; } = null!;
        public DateTime StartAt { get; set; }
        public int DurationMinutes { get; set; }
        public int ParticipantCount { get; set; }
        public bool IsRunning { get; set; }
        public bool HasJoined { get; set; }

    }
}
