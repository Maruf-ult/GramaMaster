using GramaMaster.Domain.Entities;
using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Persistence
{
    public interface IExamRepository
    {
        Task<ExamAttempt?> GetAttemptByIdAsync(Guid attemptId);

        Task<ExamAttempt?> GetStudentAttemptAsync(
            Guid studentId,
            Guid contestId);

        Task<ExamAttempt?> GetAttemptWithAnswersAsync(Guid attemptId);

        Task CreateAttemptAsync(ExamAttempt attempt);

        void UpdateAttempt(ExamAttempt attempt);
        Task AddAnswerAsync(AnswerSubmission answer);

        Task<List<AnswerSubmission>> GetAttemptAnswersAsync(Guid attemptId);
        Task<AnswerSubmission?> GetAnswerAsync(
            Guid attemptId,
            Guid problemId);

        Task<double> CalculateScoreAsync(Guid attemptId);

        Task<int> GetCorrectAnswerCountAsync(Guid attemptId);

        Task<List<ExamAttempt>> GetLeaderboardAsync(Guid contestId);

        Task<bool> HasStudentAlreadyTakenContestAsync(
            Guid studentId,
            Guid contestId);
    }
}
