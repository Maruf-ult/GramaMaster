using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Domain.Entities;
using GramaMaster.Domain.Enums;
using GramaMaster.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Infrastructure.Persistence.Repositories
{
    public class ProblemRepository:GenericRepository<Problem>,IProblemRepository
    {
        private readonly GramaMasterDbContext _dbContext;

        public ProblemRepository(GramaMasterDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Problem>> GetByTopicAsync(Guid topicId)
        {
            return await _dbContext.Problems
                .Include(x => x.Topic)
                .Include(x => x.ProblemOptions)
                .Where(x => x.TopicId == topicId && !x.IsDeleted)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Problem>> GetPracticeProblemsAsync(Guid curriculumId,Guid topicId,DifficultyType difficulty, int count)
        {
            return await _dbContext.Problems
                .Include(x => x.Topic)
                .Include(x => x.ProblemOptions)
                .Where(x => x.Topic.CurriculumId == curriculumId &&
                 x.TopicId == topicId && x.Difficulty == difficulty && x.ContestId == null && !x.IsDeleted)
                .Take(count)
                .ToListAsync();

        }

        public async Task<List<Problem>> GetRandomPracticeProblemsAsync( Guid curriculumId,Guid topicId,DifficultyType difficulty,int count)
        {
            return await _dbContext.Problems
               .Include(x => x.Topic)
               .Include(x => x.ProblemOptions)
               .Where(x => x.Topic.CurriculumId == curriculumId &&
                x.TopicId == topicId && x.Difficulty == difficulty && x.ContestId == null && !x.IsDeleted)
               .OrderBy(x => Guid.NewGuid())
               .Take(count)
               .ToListAsync();
        }

        public async Task<List<Problem>> GetContestProblemsAsync(Guid contestId)
        {
            return await _dbContext.Problems
                .Include(x => x.Topic)
                .Include(x => x.ProblemOptions)
                .Where(x => x.ContestId == contestId && !x.IsDeleted)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<Problem?> GetProblemWithOptionsAsync(Guid problemId)
        {
            return await _dbContext.Problems
                .Include(x => x.Topic)
                .Include(x => x.ProblemOptions)
                .FirstOrDefaultAsync(x => x.Id == problemId && !x.IsDeleted);
        }

        public async Task<List<Problem>> GetTeacherProblemsAsync(Guid teacherUserId)
        {
            return await _dbContext.Problems
                .Include(x => x.Topic)
                .Include(x => x.ProblemOptions)
                .Where(x => x.CreatedByUserId == teacherUserId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Problem>> SearchProblemsAsync(string keyword)
        {
            keyword = keyword.Trim().ToLower();

            return await _dbContext.Problems
                .Include(x => x.Topic)
                .Include(x => x.ProblemOptions)
                .Where(x =>
                    !x.IsDeleted &&
                    (EF.Functions.ILike(x.QuestionText, $"%{keyword}%") ||
                     EF.Functions.ILike(x.Topic.Name, $"%{keyword}%")))
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetProblemCountByTopicAsync(Guid topicId)
        {
            return await _dbContext.Problems
               .CountAsync(x =>
                   !x.IsDeleted &&
                   x.TopicId == topicId);
        }

        public async Task<int>GetPracticeProbCountByStudentIdAsync(Guid studentId)
        {
            return await _dbContext.Problems
                .CountAsync(x => !x.IsDeleted && x.AnswerSubmissions.Any(pa => pa.IsCorrect && pa.ExamAttemptId == studentId));
        }
        public async Task<int> GetOverallAccuracyByStudentIdAsync(Guid studentId)
        {
            var totalAttempts = await _dbContext.Problems
                .CountAsync(x => !x.IsDeleted && x.AnswerSubmissions.Any(pa => pa.ExamAttemptId == studentId));
            if (totalAttempts == 0)
                return 0;
            var correctAttempts = await _dbContext.Problems
                .CountAsync(x => !x.IsDeleted && x.AnswerSubmissions.Any(pa => pa.IsCorrect && pa.ExamAttemptId == studentId));
            return (int)((double)correctAttempts / totalAttempts * 100);
        }
        

        }
    }
}
