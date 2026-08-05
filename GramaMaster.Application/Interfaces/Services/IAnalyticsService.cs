using GramaMaster.Application.DTOs.Analytics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface IAnalyticsService
    {
        Task<AdminDashboardDto> GetAdminDashboardAsync();

        Task<TeacherDashboardDto> GetTeacherDashboardAsync(Guid teacherId);

        Task<StudentDashboardDto> GetStudentDashboardAsync(Guid studentId);

        Task<List<TopicStatisticsDto>> GetTopicStatisticsAsync();

        Task<List<PerformanceTrendDto>> GetPerformanceTrendAsync(Guid studentId);

        Task<List<MonthlyProgressDto>> GetMonthlyProgressAsync(Guid studentId);
    }
}
