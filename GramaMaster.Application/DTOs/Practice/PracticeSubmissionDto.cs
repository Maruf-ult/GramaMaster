using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Practice
{
    public class PracticeSubmissionDto
    {
        public Guid StudentId { get; set; }

        public List<PracticeAnswerDto> Answers { get; set; } = new();
    }

    public class PracticeAnswerDto
    {
        public Guid ProblemId { get; set; }

        public string StudentAnswer { get; set; } = string.Empty;
    }
}
