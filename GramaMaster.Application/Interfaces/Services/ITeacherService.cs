using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Teacher;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface ITeacherService
    {
        Task<ApiResponse<TeacherProfileDto>> GetProfileAsync(Guid teacherId);

        Task<ApiResponse<bool>> UpdateProfileAsync(
            Guid teacherId,
            UpdateTeacherProfileDto dto);

        Task<ApiResponse<TeacherDashboardDto>> GetDashboardAsync(Guid teacherId);

        Task<ApiResponse<TeacherAnalyticsDto>> GetAnalyticsAsync(Guid teacherId);


    }
}
