using GramaMaster.Domain.Entities;
using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Contests
{
    public class ContestDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ContestType ContestType { get; set; }
        public string Curriculum { get; set; } = string.Empty;
        public User CreatedBy { get; set; } = null!;
        public Guid? TeamId { get; set; }
        public string? TeamName { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public int DurationMinutes { get; set; }
        public int TotalProblems { get; set; }

        public int TotalParticipants { get; set; }
    }
}
