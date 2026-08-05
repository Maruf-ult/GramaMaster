using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Domain.Entities;
using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface IUserService : IGenericService<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);

        Task<User?> GetWithStudentAsync(Guid id);

        Task<User?> GetWithTeacherAsync(Guid id);
        Task<List<User>> GetUsersByRoleAsync(UserRole role, QueryDto query);
        Task<List<Teacher>> GetTeachersAsync(QueryDto query);
        Task<List<Student>> GetStudentsAsync(QueryDto query);


    }
}
