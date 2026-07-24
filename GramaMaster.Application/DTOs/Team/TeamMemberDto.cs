using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Team
{
    public class TeamMemberDto
    {
        public Guid StudentId { get; set; }

        public string FullName { get; set; } = null!;

        public string InstitutionName { get; set; } = null!;

        public CurriculumType Curriculum { get; set; }

        public string? ProfileImageUrl { get; set; }

        public DateTime JoinedAt { get; set; }
    }
}
