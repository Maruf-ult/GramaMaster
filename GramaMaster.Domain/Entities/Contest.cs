using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Domain.Entities
{
    public class Contest:BaseEntity
    {
 
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ContestType ContestType { get; set; }
        public Guid CurriculumId { get; set; }
        public Curriculum Curriculum { get; set; } = null!;
        public Guid CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;
        public Guid? TeamId { get; set; }
        public Team? Team { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public int DurationMinutes { get; set; }

        public ICollection<Problem>Problems { get; set; } = new List<Problem>();
        public ICollection<ExamAttempt> ExamAttempts { get; set; } = new List<ExamAttempt>();


    }
}
