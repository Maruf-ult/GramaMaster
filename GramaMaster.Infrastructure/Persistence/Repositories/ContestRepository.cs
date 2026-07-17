using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Domain.Entities;
using GramaMaster.Domain.Enums;
using GramaMaster.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GramaMaster.Infrastructure.Persistence.Repositories
{
    public class ContestRepository:GenericRepository<Contest>,IContestRepository
    {
        private readonly GramaMasterDbContext _dbContext;

        public ContestRepository(GramaMasterDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Contest>> GetGlobalContestsAsync(QueryDto query)
        {
            IQueryable<Contest> contests = _dbContext.Contests
                .Include(x => x.Curriculum)
                .Include(x => x.CreatedByUser)
                .Where(x => !x.IsDeleted && x.ContestType == ContestType.Global);


            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                contests = contests.Where(x =>
                x.Title.Contains(query.Search) ||
                x.Description.Contains(query.Search));
            }

            contests = query.SortBy?.ToLower() switch
            {
                "title" => contests.OrderBy(x => x.Title),
                "title_desc" => contests.OrderByDescending(x => x.Title),
                "start" => contests.OrderBy(x => x.StartAt),
                "start_desc" => contests.OrderByDescending(x => x.StartAt),
                "end" => contests.OrderBy(x => x.EndAt),
                "end_desc" => contests.OrderByDescending(x => x.EndAt),
                _ => contests.OrderByDescending(x => x.StartAt)
            };

            contests = contests
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize);

            return await contests.ToListAsync();
        }
        public async Task<List<Contest>> GetTeamContestsAsync(Guid teamId,QueryDto query)
        {
            IQueryable<Contest>contests = _dbContext.Contests
                .Include(x => x.Team)
                .Include(x => x.CreatedByUser)
                .Where(x => !x.IsDeleted && x.TeamId == teamId);


            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                contests = contests.Where(x =>
                x.Title.Contains(query.Search) ||
                x.Description.Contains(query.Search));
            }

            contests = query.SortBy?.ToLower() switch
            {
                "title" => contests.OrderBy(x => x.Title),
                "title_desc" => contests.OrderByDescending(x => x.Title),
                "start" => contests.OrderBy(x => x.StartAt),
                "start_desc" => contests.OrderByDescending(x => x.StartAt),
                "end" => contests.OrderBy(x => x.EndAt),
                "end_desc" => contests.OrderByDescending(x => x.EndAt),
                _ => contests.OrderByDescending(x => x.StartAt)
            };

            contests = contests
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize);

            return await contests.ToListAsync();
        }
        public async Task<Contest?> GetContestWithProblemsAsync(Guid contestId)
        {
            return await _dbContext.Contests
                .Include(x => x.Curriculum)
                .Include(x => x.Team)
                .Include(x => x.CreatedByUser)
                .Include(x => x.Problems)
                    .ThenInclude(p => p.ProblemOptions)
                .FirstOrDefaultAsync(x => x.Id == contestId && !x.IsDeleted);
        }
        public async Task<bool> IsContestRunningAsync(Guid contestId)
        {
            var now = DateTime.UtcNow;

            return await _dbContext.Contests
                .AnyAsync(x => x.Id == contestId && !x.IsDeleted
                && x.StartAt <= now && x.EndAt >= now);

        }
        public async Task<List<Contest>> GetUpcomingContestsAsync()
        {
            var now = DateTime.UtcNow;

            return await _dbContext.Contests
                .Include(x => x.Curriculum)
                .Include(x => x.CreatedByUser)
                .Include(x => x.Team)
                .Where(x => !x.IsDeleted && x.StartAt>now)
                .OrderBy(x => x.StartAt)
                .ToListAsync();
        }

        public async Task<List<Contest>> GetRunningContestsAsync()
        {
            var now = DateTime.UtcNow;

            return await _dbContext.Contests
                .Include(x => x.Curriculum)
                .Include(x => x.CreatedByUser)
                .Include(x => x.Team)
                .Where(x => !x.IsDeleted && x.StartAt<=now && x.EndAt>=now)
                .OrderBy(x => x.StartAt)
                .ToListAsync();
        }

        public async Task<List<Contest>> GetCompletedContestsAsync(QueryDto query)
        {
            var now = DateTime.UtcNow;

            IQueryable<Contest> contests = _dbContext.Contests
                .Include(x => x.Curriculum)
                .Include(x => x.CreatedByUser)
                .Include(x => x.Team)
                .Where(x => !x.IsDeleted && x.EndAt > now);


            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                contests = contests.Where(x =>
                x.Title.Contains(query.Search) ||
                x.Description.Contains(query.Search));
            }

            contests = query.SortBy?.ToLower() switch
            {
                "title" => contests.OrderBy(x => x.Title),
                "title_desc" => contests.OrderByDescending(x => x.Title),
                "start" => contests.OrderBy(x => x.StartAt),
                "start_desc" => contests.OrderByDescending(x => x.StartAt),
                "end" => contests.OrderBy(x => x.EndAt),
                "end_desc" => contests.OrderByDescending(x => x.EndAt),
                _ => contests.OrderByDescending(x => x.StartAt)
            };

            contests = contests
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize);

            return await contests.ToListAsync();

        }

        public async Task<Contest?> GetContestWithAttemptsAsync(Guid contestId)
        {
            return await _dbContext.Contests
                 .Include(x => x.Curriculum)
                 .Include(x => x.Team)
                 .Include(x => x.CreatedByUser)
                 .Include(x => x.ExamAttempts)
                   .ThenInclude(a => a.Student)
                     .ThenInclude(s => s.User)
                 .FirstOrDefaultAsync(x => x.Id == contestId);
        }

        public async Task<List<Contest>> GetContestsByCurriculumAsync(Guid curriculumId)
        {
            return await _dbContext.Contests
                .Include(x => x.Curriculum)
                .Include(x => x.Team)
                .Include(x => x.CreatedByUser)
                .Where(x => x.CurriculumId == curriculumId && !x.IsDeleted)
                .OrderByDescending(x => x.StartAt)
                .ToListAsync();
        }

        public async Task<List<Contest>> GetTeacherContestsAsync(Guid teacherUserId, QueryDto query)
        {
            IQueryable<Contest> contests = _dbContext.Contests
                .Include(x => x.Curriculum)
                .Include(x => x.Team)
                .Where(x => x.CreatedByUserId == teacherUserId && !x.IsDeleted);


            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                contests = contests.Where(x =>
                x.Title.Contains(query.Search) ||
                x.Description.Contains(query.Search));
            }

            contests = query.SortBy?.ToLower() switch
            {
                "title" => contests.OrderBy(x => x.Title),
                "title_desc" => contests.OrderByDescending(x => x.Title),
                "created" => contests.OrderBy(x => x.CreatedAt),
                "created_desc" => contests.OrderByDescending(x => x.CreatedAt),
                "end" => contests.OrderBy(x => x.EndAt),
                "end_desc" => contests.OrderByDescending(x => x.EndAt),
                _ => contests.OrderByDescending(x => x.StartAt)
            };

            contests = contests
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize);

            return await contests.ToListAsync();

        }

        public async Task<List<Contest>> GetStudentAvailableContestsAsync(Guid studentId)
        {
            var student = await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == studentId);
            if (student == null)
            {
                return new List<Contest>();
            }
            return await _dbContext.Contests
                .Include(x => x.Curriculum)
                .Include(x => x.CreatedByUser)
                .Include(x => x.Team)
                .Where(x =>
                !x.IsDeleted && x.CurriculumId == student.CurriculumId &&
                (
                   x.ContestType == ContestType.Global ||
                   (x.Team != null && x.Team.TeamMembers.Any(x => x.StudentId == studentId)
                )))
                .OrderBy(x => x.StartAt)
                .ToListAsync();

        }

        public async Task<bool> HasStudentJoinedAsync(Guid contestId, Guid studentId)
        {
            return await _dbContext.ExamAttempts
                .AnyAsync(x => x.ContestId == contestId &&
                x.StudentId == studentId);
        }

        public async Task<bool> HasStudentSubmittedAsync(Guid contestId, Guid studentId)
        {
            return await _dbContext.ExamAttempts
                .AnyAsync(x => x.ContestId == contestId && x.StudentId == studentId && x.SubmittedAt != null);
        }

        public async Task<int> GetContestParticipantCountAsync(Guid contestId)
        {
            return await _dbContext.ExamAttempts
                .CountAsync(x => x.ContestId == contestId);
        }
    }
}
