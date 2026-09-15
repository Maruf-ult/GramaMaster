using GramaMaster.Application.DTOs.Common;
using GramaMaster.Domain.Entities;
using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Persistence
{
    public interface IExamRepository
    {
        Task<ExamAttempt?> GetStudentAttemptAsync(Guid studentId, Guid contestId);

        Task<ExamAttempt?> GetAttemptWithAnswersAsync(Guid attemptId);

        Task<AnswerSubmission?> GetAnswerAsync(Guid attemptId, Guid problemId);

        Task<List<AnswerSubmission>> GetAttemptAnswersAsync(Guid attemptId);

        Task<double> CalculateScoreAsync(Guid attemptId);

        Task<int> GetCorrectAnswerCountAsync(Guid attemptId);

        Task<List<ExamAttempt>> GetLeaderboardAsync(Guid contestId,QueryDto query);

        Task<bool> HasStudentAlreadyTakenContestAsync(Guid studentId, Guid contestId);
       
    }
}
