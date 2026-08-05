using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Teacher;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface ITeacherService
    {
        Task<TeacherProfileDto> GetProfileAsync(Guid teacherId);

        Task<ApiResponse<bool>> UpdateProfileAsync(
            Guid teacherId,
            UpdateTeacherProfileDto dto);

        Task<TeacherDashboardDto> GetDashboardAsync(Guid teacherId);

        Task<TeacherAnalyticsDto> GetAnalyticsAsync(Guid teacherId);


    }
}
