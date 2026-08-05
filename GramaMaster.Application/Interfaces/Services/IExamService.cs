using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Exams;
using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface IExamService
    {
        Task<ExamAttemptDto> StartExamAsync(
            Guid studentId,
            StartExamDto dto);

        Task<ExamResultDto> SubmitExamAsync(
            Guid studentId,
            SubmitExamDto dto);

        Task<ExamAttemptDto> GetAttemptAsync(Guid attemptId);

        Task<List<LeaderboardDto>> GetLeaderboardAsync(
            Guid contestId,
            QueryDto query);

        Task<List<AnswerReviewDto>> ReviewAnswersAsync(Guid attemptId);
    }
}
