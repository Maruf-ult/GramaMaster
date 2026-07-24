using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Topics
{
    public class TopicAnalyticsDto
    {
        public Guid TopicId { get; set; }
        public string TopicName { get; set; } = string.Empty;
        public double Accuracy { get; set; }
        public int TotalAttempts { get; set; }
    }
}
