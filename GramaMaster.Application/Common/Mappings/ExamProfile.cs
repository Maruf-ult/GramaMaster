using AutoMapper;
using GramaMaster.Application.DTOs.Exams;
using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Common.Mappings
{
    public class ExamProfile:Profile
    {
        public ExamProfile()
        {
            CreateMap<ExamAttemptDto, ExamAttempt>();
            CreateMap<ExamAttempt, ExamResultDto>();
            CreateMap<AnswerSubmission, AnswerReviewDto>();

        }
    }
}
