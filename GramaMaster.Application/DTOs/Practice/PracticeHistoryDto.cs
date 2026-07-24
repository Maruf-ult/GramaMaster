using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Practice
{

    public class PracticeHistoryDto
    {
        public Guid Id { get; set; }

        public string Topic { get; set; } = string.Empty;

        public double Score { get; set; }

        public DateTime CompletedAt { get; set; }
    }
}
