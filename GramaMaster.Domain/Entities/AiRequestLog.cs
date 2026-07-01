using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Domain.Entities
{
    public class AiRequestLog:BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public string? FeatureName { get; set; }
        public string? Prompt { get; set; }
        public string? Response { get; set; }
        public int TokensUsed { get; set; }
        public double Cost { get; set; }
    }
}
