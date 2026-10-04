using GramaMaster.Application.DTOs.Analytics;
using GramaMaster.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface IAnalyticsService
    {
        Task<ApiResponse<AdminDashboardDto>> GetAdminDashboardAsync();

        Task<ApiResponse<List<TopicStatisticsDto>>> GetTopicStatisticsAsync();

        Task<ApiResponse<List<PerformanceTrendDto>>> GetPerformanceTrendAsync(Guid studentId);

        Task<ApiResponse<List<MonthlyProgressDto>>> GetMonthlyProgressAsync(Guid studentId);
    }
}
