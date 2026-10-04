using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Contests;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface IContestService
    {
        Task<ApiResponse<ContestDto>> CreateContestAsync(
            Guid userId,
            CreateContestDto dto);

        Task<ApiResponse<bool>> UpdateContestAsync(
            Guid contestId,
            UpdateContestDto dto);

        Task<ApiResponse<bool>> DeleteContestAsync(Guid contestId);

        Task<ApiResponse<ContestDetailsDto>> GetContestDetailsAsync(Guid contestId);

        Task<ApiResponse<PagedResultDto<ContestCardDto>>> GetGlobalContestsAsync(QueryDto query);

        Task<ApiResponse<PagedResultDto<ContestCardDto>>> GetTeacherContestsAsync(
            Guid teacherId,
            QueryDto query);

        Task<ApiResponse<PagedResultDto<ContestCardDto>>> GetStudentAvailableContestsAsync(
            Guid studentId,
            QueryDto query);

        Task<ApiResponse<List<ContestLeaderBoardDto>>> GetLeaderboardAsync(Guid contestId);

        Task<ApiResponse<ContestAnalyticsDto>> GetAnalyticsAsync(Guid contestId);
    }
}