using GramaMaster.Domain.Entities;
using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Contests
{
    public class ContestDetailsDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Curriculum { get; set; } = string.Empty;

        public string CreatedBy { get; set; } = string.Empty;

        public string? TeamName { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public int DurationMinutes { get; set; }

        public int TotalProblems { get; set; }

        public bool HasJoined { get; set; }

        public bool HasSubmitted { get; set; }

        public bool IsRunning { get; set; }
    }
}
