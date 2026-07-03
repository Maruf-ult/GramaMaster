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

        public async Task<List<User>> GetUsersByRoleAsync(UserRole role)
        {
            return await _dbContext.Users
                .Where(x => x.Role == role)
                .OrderBy(x => x.FullName)
                .ToListAsync();
        }

        public async Task<List<Teacher>> GetTeachersAsync()
        {
            return await _dbContext.Teachers
                .Include(x => x.User)
                .Where(x => !x.IsDeleted && !x.User.IsDeleted && x.User.IsActive)
                .OrderBy(x => x.User.FullName)
                .ToListAsync();
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _dbContext.Students
                .Include(x => x.User)
                .Where(x => !x.IsDeleted && !x.User.IsDeleted && x.User.IsActive)
                .OrderBy(x => x.User.FullName)
                .ToListAsync();
        }
    }
}
