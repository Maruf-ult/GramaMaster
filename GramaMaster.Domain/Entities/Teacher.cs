using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Domain.Entities
{
    public class Teacher:BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public string? ProfileImageUrl { get; set; }
        public required double SscResult { get; set; } 
        public StudyGroup SscGroup { get; set; }
        public EducationBoard SscBoard { get; set; } 
        public required double HscResult { get; set; }
        public StudyGroup HscGroup { get; set; }
        public EducationBoard HscBoard { get; set; }
        public required string Department { get; set; }
        public required string University { get; set; }

        public ICollection<Team> Teams { get; set; } = new List<Team>();
    }
}
