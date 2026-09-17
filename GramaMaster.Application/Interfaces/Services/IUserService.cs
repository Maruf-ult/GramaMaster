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
        Task<ApiResponse<User?>> GetByEmailAsync(string email);
        Task<ApiResponse<bool>> ExistsByEmailAsync(string email);
        Task<ApiResponse<User?>> GetWithStudentAsync(Guid id);
        Task<ApiResponse<User?>> GetWithTeacherAsync(Guid id);
        Task<ApiResponse<List<User>>> GetUsersByRoleAsync(UserRole role, QueryDto query);
        Task<ApiResponse<List<Teacher>>> GetTeachersAsync(QueryDto query);
        Task<ApiResponse<List<Student>>> GetStudentsAsync(QueryDto query);

    }
}
