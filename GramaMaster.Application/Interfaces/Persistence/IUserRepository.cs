using GramaMaster.Domain.Entities;
using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Persistence
{
    public interface IUserRepository:IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);

        Task<User?> GetWithStudentAsync(Guid id);

        Task<User?> GetWithTeacherAsync(Guid id);
        Task<List<User>> GetUsersByRoleAsync(UserRole role);
        Task<List<Teacher>> GetTeachersAsync();
        Task<List<Student>> GetStudentsAsync();


    }
}
