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
        Task<ApiResponse<ExamAttemptDto>> StartExamAsync(
            Guid studentId,
            StartExamDto dto);

        Task<ApiResponse<ExamResultDto>> SubmitExamAsync(
            Guid studentId,
            SubmitExamDto dto);

        Task<ApiResponse<ExamAttemptDto>> GetAttemptAsync(Guid studentId,Guid attemptId);

        Task<ApiResponse<List<LeaderboardDto>>> GetLeaderboardAsync(
            Guid contestId,
            QueryDto query);

        Task<ApiResponse<List<AnswerReviewDto>>> ReviewAnswersAsync(Guid attemptId);
    }
}
