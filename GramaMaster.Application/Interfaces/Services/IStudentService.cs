using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Student;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface IStudentService
    {
        Task<StudentProfileDto> GetProfileAsync(Guid studentId);

        Task<ApiResponse<bool>> UpdateProfileAsync(
            Guid studentId,
            UpdateStudentProfileDto dto);

        Task<StudentDashboardDto> GetDashboardAsync(Guid studentId);

        Task<StudentAnalyticsDto> GetAnalyticsAsync(Guid studentId);

        Task<List<StudentProgressDto>> GetProgressAsync(Guid studentId);

        Task<List<StudentTopicProgressDto>> GetTopicProgressAsync(Guid studentId);
    }
}
