using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Exams
{
    public class SubmitAnswerDto
    {
        public Guid ProblemId { get; set; }

        public string StudentAnswer { get; set; } = string.Empty;
    }
}
