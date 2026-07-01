using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Domain.Entities
{
    public class Student:BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public string? ProfileImageUrl { get; set; }
        public string InstitutionName { get; set; } = null!;
        public CurriculumType CurriculumType { get; set; }

        public Guid CurriculumId { get; set; }
        public Curriculum Curriculum { get; set; } = null!;
        public StudyGroup Group { get; set; }
        public int ExamBatchYear { get; set; } 
        public EducationBoard Board { get; set; }

        public ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
        public ICollection<ExamAttempt> ExamAttempts { get; set; } = new List<ExamAttempt>();
        public ICollection<ChatSession> ChatSessions { get; set; } = new List<ChatSession>();


    }
}
