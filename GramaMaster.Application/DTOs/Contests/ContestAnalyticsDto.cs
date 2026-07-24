using GramaMaster.Application.DTOs.Topics;
using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Contests
{
    public class ContestAnalyticsDto
    {
        public Guid ContestId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int TotalParticipants { get; set; }
        public double AverageScore { get; set; }
        public double HighestScore { get; set; }
        public double LowestScore { get; set; }
        public double PassRate { get; set; }
        public List<TopicAnalyticsDto> Topics { get; set; } = new();


    }
}
