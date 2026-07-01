using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Domain.Entities
{
    public class TeamMember:BaseEntity
    {
        public Guid TeamId { get; set; }
        public Team Team { get; set; } = null!;
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public DateTime JoinedAt { get; set; }

    }
}
