using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Domain.Entities
{
    public class Problem : BaseEntity
    {
        public Guid TopicId { get; set; }
        public Topic Topic { get; set; } = null!;

        public Guid? ContestId { get; set; }
        public Contest? Contest { get; set; }

        public Guid CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;

        public ProblemType ProblemType { get; set; }

        public string QuestionText { get; set; } = null!;

        public string CorrectAns { get; set; } = null!;

        public string Explanation { get; set; } = null!;

        public DifficultyType Difficulty { get; set; }

        public bool IsAiGenerated { get; set; }

        public ICollection<ProblemOptions> ProblemOptions { get; set; }  = new List<ProblemOptions>();
        public ICollection<AnswerSubmission> AnswerSubmissions { get; set; } = new List<AnswerSubmission>();
    }
}
