using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.AI
{
    public class AIUsageDto
    {
        public string FeatureName { get; set; } = string.Empty;

        public int TotalRequests { get; set; }

        public int TotalTokens { get; set; }

        public double TotalCost { get; set; }

        public double AverageTokens { get; set; }
    }
}
