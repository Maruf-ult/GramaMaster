using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Domain.Entities
{
    public class Team:BaseEntity
    {
        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string JoinCode { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public int MaxStudents { get; set; } = 100;

        public ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();

        public ICollection<Contest> Contests { get; set; } = new List<Contest>();

    }
}
