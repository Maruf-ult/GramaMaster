using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Student;
using GramaMaster.Application.DTOs.Teacher;
using GramaMaster.Application.DTOs.Users; 
using GramaMaster.Domain.Enums;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<ApiResponse<UserDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<UserDto>> GetByEmailAsync(string email);
        Task<ApiResponse<bool>> ExistsByEmailAsync(string email);
        Task<ApiResponse<List<UserDto>>> GetUsersAsync(
            QueryDto query);
        Task<ApiResponse<List<UserDto>>> GetUsersByRoleAsync(
            UserRole role,
            QueryDto query);
        Task<ApiResponse<List<UserDto>>> GetTeachersAsync(
            QueryDto query);

        Task<ApiResponse<List<UserDto>>> GetStudentsAsync(
            QueryDto query);

        Task<ApiResponse<TeacherProfileDto>> GetTeacherAsync(
            Guid userId);

        Task<ApiResponse<StudentProfileDto>> GetStudentAsync(
            Guid userId);

        Task<ApiResponse<bool>> UpdateStatusAsync(
            Guid userId,
            bool isActive);

        Task<ApiResponse<bool>> DeleteAsync(
            Guid userId);
    }
}
