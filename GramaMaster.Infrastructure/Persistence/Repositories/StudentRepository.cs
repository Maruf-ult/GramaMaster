using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Domain.Entities;
using GramaMaster.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Infrastructure.Persistence.Repositories
{
    public class StudentRepository:GenericRepository<Student>,IStudentRepository
    {
        private readonly GramaMasterDbContext _dbContext;

        public StudentRepository(GramaMasterDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Student?> GetStudentWithDetailsAsync(Guid studentIdOrUserId)
        {
            return await _dbContext.Students
                .Include(s => s.User)
                .Include(s => s.Curriculum)
                .FirstOrDefaultAsync(s => s.Id == studentIdOrUserId || s.UserId == studentIdOrUserId && !s.IsDeleted);

        }
        public async Task<List<ExamAttempt>> GetStudentAttemptsWithAnswersAsync(Guid studentId)
        {
            return await _dbContext.ExamAttempts
                .Include(ea => ea.AnswerSubmissions)
                .ThenInclude(p => p.Problem)
                .Where(a => a.StudentId == studentId && !a.IsDeleted)
                .ToListAsync();
        }
        public async Task<List<Topic>> GetCurriculumTopicsWithProblemsAsync(Guid curriculumId)
        {
            var topics = await _dbContext.Topics
                .Include(t => t.Problems)
                .Where(t => t.CurriculumId == curriculumId && !t.IsDeleted)
                .ToListAsync();

            if (!topics.Any())
            {
                topics = await _dbContext.Topics
                    .Include(t => t.Problems)
                    .Where(t =>!t.IsDeleted)
                    .ToListAsync();
            }
            return topics;
        }
    }
}
