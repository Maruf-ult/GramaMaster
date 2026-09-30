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
        
        public async Task<List<Team>>GetTeamsByStudentIdAsync(Guid studentId)
        {
            return await _dbContext.Teams
                 .Where(x => x.TeamMembers.Any(tm => tm.StudentId == studentId))
                 .ToListAsync();
        }

        public async Task<Team?> GetByJoinCodeAsync(string joinCode)
        {
            return await _dbContext.Teams
                .Include(x => x.TeamMembers)
                .FirstOrDefaultAsync(x => x.JoinCode == joinCode && !x.IsDeleted && x.IsActive);
        }

        public async Task<List<ExamAttempt>> GetStudentHistory(Guid studentId)
        {
            return await _dbContext.ExamAttempts
                .Include(p => p.AnswerSubmissions)
                .ThenInclude(p => p.Problem)
                .ThenInclude(p => p.Topic)
                .Where(x => x.StudentId== studentId && !x.IsDeleted && x.SubmittedAt!=null)
                .OrderByDescending(x => x.SubmittedAt)
                .ToListAsync();
        }
       
    }
}
