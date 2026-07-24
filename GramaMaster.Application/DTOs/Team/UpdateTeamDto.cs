using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Team
{
    public class UpdateTeamDto
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

    }
}
