using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Team
{
    public class TeamDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string JoinCode { get; set; } = null!;

        public Guid TeacherId { get; set; }

        public string TeacherName { get; set; } = null!;

        public int MemberCount { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<TeamMemberDto> Members { get; set; } = new();
    }
}
