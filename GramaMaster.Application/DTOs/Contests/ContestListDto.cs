using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Contests
{
    public class ContestListDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public ContestType ContestType { get; set; }

        public string Curriculum { get; set; } = string.Empty;

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }
    }
}
