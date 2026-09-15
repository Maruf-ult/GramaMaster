using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Student;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface IStudentService
    {
        Task<ApiResponse<StudentProfileDto>> GetProfileAsync(Guid studentId);

        Task<ApiResponse<bool>> UpdateProfileAsync(Guid studentId, UpdateStudentProfileDto dto);

        Task<ApiResponse<StudentDashboardDto>> GetDashboardAsync(Guid studentId);

        Task<ApiResponse<StudentAnalyticsDto>> GetAnalyticsAsync(Guid studentId);

        Task<ApiResponse<StudentProgressDto>> GetProgressAsync(Guid studentId);

        Task<ApiResponse<List<StudentTopicProgressDto>>> GetTopicProgressAsync(Guid studentId);
    }
}
