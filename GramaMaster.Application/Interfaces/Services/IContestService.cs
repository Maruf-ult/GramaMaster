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

        Task<ContestDetailsDto> GetContestDetailsAsync(Guid contestId);

        Task<PagedResultDto<ContestCardDto>> GetGlobalContestsAsync(QueryDto query);

        Task<PagedResultDto<ContestCardDto>> GetTeacherContestsAsync(
            Guid teacherId,
            QueryDto query);

        Task<PagedResultDto<ContestCardDto>> GetStudentAvailableContestsAsync(
            Guid studentId,
            QueryDto query);

        Task<List<ContestLeaderBoardDto>> GetLeaderboardAsync(Guid contestId);

        Task<ContestAnalyticsDto> GetAnalyticsAsync(Guid contestId);
    }
}