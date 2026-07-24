using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Exams
{
    public class SubmitExamDto
    {
        public Guid AttemptId { get; set; }

        public List<SubmitAnswerDto> Answers { get; set; } = new();
    }
}
