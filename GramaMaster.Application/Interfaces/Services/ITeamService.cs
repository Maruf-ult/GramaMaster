using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Team;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface ITeamService
    {
        Task<ApiResponse<TeamDto>> CreateTeamAsync(
            Guid teacherId,
            CreateTeamDto dto);

        Task<ApiResponse<bool>> UpdateTeamAsync(
            Guid teamId,
            UpdateTeamDto dto);

        Task<ApiResponse<bool>> DeleteTeamAsync(Guid teamId);

        Task<ApiResponse<bool>> JoinTeamAsync(
            Guid studentId,
            JoinTeamDto dto);

        Task<TeamDto> GetTeamAsync(Guid teamId);

        Task<List<TeamDto>> GetTeacherTeamsAsync(Guid teacherId);

        Task<List<TeamDto>> GetStudentTeamsAsync(Guid studentId);

        Task<List<TeamMemberDto>> GetMembersAsync(Guid teamId);

        Task<ApiResponse<bool>> RemoveStudentAsync(
            Guid teamId,
            Guid studentId);
    }
}
