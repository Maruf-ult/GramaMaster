using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Curriculum
{
    public class CurriculumDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TopicCount { get; set; }
        public int StudentCount { get; set; }
        public int ContestCount { get; set; }
    }
}
