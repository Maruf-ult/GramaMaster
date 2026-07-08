using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Domain.Entities;
using GramaMaster.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Infrastructure.Persistence.Repositories
{
    public class ExamRepository:GenericRepository<ExamAttempt>,IExamRepository
    {
        private readonly GramaMasterDbContext _dbContext;

        public ExamRepository(GramaMasterDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ExamAttempt?> GetStudentAttemptAsync(Guid studentId, Guid contestId)
        {
            return await _dbContext.ExamAttempts
                .Include(x => x.Student)
                   .ThenInclude(x => x.User)
                .Include(x => x.Contest)
                .FirstOrDefaultAsync(x => x.StudentId == studentId && x.ContestId == contestId && !x.IsDeleted);
        }

        public async Task<ExamAttempt?> GetAttemptWithAnswersAsync(Guid attemptId)
        {
            return await _dbContext.ExamAttempts
                .Include(x => x.AnswerSubmissions)
                   .ThenInclude(x => x.Problem)
                      .ThenInclude(x => x.ProblemOptions)
                .Include(x => x.Student)
                   .ThenInclude(x => x.User)
                 .Include(x => x.Contest)
                .FirstOrDefaultAsync(x => x.Id == attemptId && !x.IsDeleted);
        }

        public async Task<AnswerSubmission?> GetAnswerAsync(Guid attemptId, Guid problemId)
        {
            return await _dbContext.AnswerSubmissions
                .Include(x => x.Problem)
                .FirstOrDefaultAsync(x => x.Id == attemptId && x.ProblemId == problemId && !x.IsDeleted);
        }

        public async Task<List<AnswerSubmission>> GetAttemptAnswersAsync(Guid attemptId)
        {

            return await _dbContext.AnswerSubmissions
                .Include(x => x.Problem)
                    .ThenInclude(x => x.ProblemOptions)
                .Where(x =>
                    x.ExamAttemptId == attemptId &&
                    !x.IsDeleted)
                .ToListAsync();

        }

        public async Task<double> CalculateScoreAsync(Guid attemptId)
        {
            var attempt = await _dbContext.ExamAttempts
                                .Include(x => x.AnswerSubmissions)
                                .FirstOrDefaultAsync(x => x.Id == attemptId);
            
            if(attempt == null || attempt.TotalQuestions == 0)
            {
                return 0;
            }

            var correctAns = attempt.AnswerSubmissions.Count(x => x.IsCorrect);

            return (double)correctAns / attempt.TotalQuestions * 100;
                
        }

        public async Task<int> GetCorrectAnswerCountAsync(Guid attemptId)
        {
            return await _dbContext.AnswerSubmissions.CountAsync(x => x.ExamAttemptId == attemptId && !x.IsDeleted && x.IsCorrect);
        }

        public async Task<List<ExamAttempt>> GetLeaderboardAsync(Guid contestId)
        {
            return await _dbContext.ExamAttempts
                .Include(x => x.Student)
                  .ThenInclude(x => x.User)
                .Where(x => x.ContestId == contestId && !x.IsDeleted)
                .OrderByDescending(x => x.Score)
                .ThenBy(x => x.SubmittedAt)
                .ToListAsync();
        }

        public async Task<bool> HasStudentAlreadyTakenContestAsync(Guid studentId, Guid contestId)
        {
            return await _dbContext.ExamAttempts
                .AnyAsync(x => x.StudentId == studentId && 
                x.ContestId == contestId && 
                !x.IsDeleted);

        }


    }
}
