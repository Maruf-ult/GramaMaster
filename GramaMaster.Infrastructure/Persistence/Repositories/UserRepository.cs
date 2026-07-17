using GramaMaster.Application.DTOs.Common;
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
    public class UserRepository:GenericRepository<User>,IUserRepository
    {
        private readonly GramaMasterDbContext _dbContext;

        public UserRepository(GramaMasterDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted);
        }
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbContext.Users.AnyAsync(x => x.Email == email && !x.IsDeleted);
        }

        public async Task<User?> GetWithStudentAsync(Guid id)
        {
            return await _dbContext.Users
                .Include(x => x.Student)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<User?> GetWithTeacherAsync(Guid id)
        {
            return await _dbContext.Users
               .Include(x => x.Teacher)
               .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<List<User>> GetUsersByRoleAsync(UserRole role,QueryDto query)
        {
            IQueryable<User> users = _dbContext.Users
                .Where(x => x.Role == role)
                .OrderBy(x => x.FullName);
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                users = users.Where(x =>
                x.FullName.Contains(query.Search) ||
                x.Email.Contains(query.Search));
            }
            users = query.SortBy?.ToLower() switch
            {
                "createdat" => users.OrderBy(x => x.CreatedAt),
                "createdat_desc" => users.OrderByDescending(x => x.CreatedAt),
                "name" => users.OrderByDescending(x => x.FullName),
                _ => users.OrderBy(x => x.FullName)
            };
            users = users
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize);

            return await users.ToListAsync();
        }

        public async Task<List<Teacher>> GetTeachersAsync(QueryDto query)
        {
            IQueryable<Teacher> teachers = _dbContext.Teachers
                .Include(x => x.User)
                .Where(x => !x.IsDeleted && !x.User.IsDeleted && x.User.IsActive)
                .OrderBy(x => x.User.FullName);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                teachers.Where(x => x.University.Contains(query.Search) ||
                 x.Department.Contains(query.Search) ||
                 x.User.FullName.Contains(query.Search) ||
                 x.User.Email.Contains(query.Search));
            }

            teachers = query.SortBy?.ToLower() switch
            {
                "university" => teachers.OrderBy(x => x.University),
                "university_desc" => teachers.OrderByDescending(x => x.University),
                "board" => teachers.OrderBy(x => x.HscBoard),
                "board_desc" => teachers.OrderByDescending(x => x.HscBoard),
                "hsc_result" => teachers.OrderBy(x => x.HscResult),
                "hsc_result_desc" => teachers.OrderByDescending(x => x.HscResult),
                _ => teachers.OrderBy(x => x.User.FullName)
            };

            teachers = teachers
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize);

            return await teachers.ToListAsync();

        }

        public async Task<List<Student>> GetStudentsAsync(QueryDto query)
        {
            IQueryable<Student> students = _dbContext.Students
                .Include(x => x.User)
                .Where(x => !x.IsDeleted && !x.User.IsDeleted && x.User.IsActive)
                .OrderBy(x => x.User.FullName);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                students = students.Where(x =>
                  x.User.FullName.Contains(query.Search) ||
                 x.User.Email.Contains(query.Search));
            }

            students = query.SortBy?.ToLower() switch
            {
                "group" => students.OrderBy(x => x.Group),
                "group_desc" => students.OrderByDescending(x => x.Group),
                "board" => students.OrderBy(x => x.Board),
                "board_desc" => students.OrderByDescending(x => x.Board),
                "curriculum" => students.OrderBy(x => x.Curriculum),
                "curriculum_desc" => students.OrderByDescending(x => x.Curriculum),
                _ => students.OrderBy(x => x.User.FullName)
            };

            students = students
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize);

            return await students.ToListAsync();

        }
    }
}
