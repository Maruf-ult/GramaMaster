using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Domain.Entities;
using GramaMaster.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Infrastructure.Persistence.Repositories
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        private readonly GramaMasterDbContext _dbContext;

        public TeacherRepository(GramaMasterDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Teacher?> GetTeacherWithDetailsAsync(Guid teacherId)
        {
            return await _dbContext.Teachers
                .Include(x => x.User)
                .Include(x => x.Teams)
                .FirstOrDefaultAsync(x => x.Id == teacherId || x.UserId == teacherId && !x.IsDeleted);
        }

       public async Task<int>GetTeacherContestCountAsync(Guid teacherId)
        {
            return await _dbContext.Contests
                .Include(x => x.Curriculum)
                .Include(x => x.Team)
                .Where(x => x.CreatedByUserId == teacherId && !x.IsDeleted)
                .CountAsync();
        }

        public async Task<int>GetTotalProblemsCreatedAsync(Guid teacherId)
        {
            return await _dbContext.Problems
                .Where(x => x.CreatedByUserId == teacherId && !x.IsDeleted)
                .CountAsync();
        }

        public async Task<List<Problem>>GetProblemsCreatedAsync(Guid teacherId)
        {
            return await _dbContext.Problems
                .Where(x => x.CreatedByUserId == teacherId && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<int>GetCompletedContestCountAsync(Guid teacherId)
        {
            var now = DateTime.UtcNow;
            return await _dbContext.Contests
                .Where(x => x.CreatedByUserId == teacherId && !x.IsDeleted && now > x.EndAt)
                .CountAsync();
        }

        public async Task<int>GetRunningContestCountAsync(Guid teacherId)
        {
            return await _dbContext.Contests
                .Where(x => x.CreatedByUserId == teacherId && !x.IsDeleted && x.StartAt < x.EndAt)
                .CountAsync();
        }

        public async Task<List<Team>>GetTeamsByTeacherId(Guid teacherId)
        {
            return await _dbContext.Teams.Where(x => x.TeacherId == teacherId && !x.IsDeleted).ToListAsync();
        }

        public async Task<bool>RemoveStudentByIdAsync(Guid studentId)
        {
            var student = await _dbContext.TeamMembers.FirstOrDefaultAsync(x => x.StudentId == studentId && !x.IsDeleted);

            if (student == null) return false;
            student.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTopicAsync(Guid topicId)
        {
            var topic = await _dbContext.Topics.FirstOrDefaultAsync(x => x.Id == topicId);

            if (topic == null) return false;
            topic.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Topic>> GetTopicByCurriculumAsync(Guid curriculumId)
        {
            return await _dbContext.Topics.Where(x => x.CurriculumId == curriculumId).ToListAsync();
        }

    }
}
