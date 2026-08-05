using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Problems;
using GramaMaster.Domain.Entities;
using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface IProblemService
    {
        Task<ApiResponse<ProblemDto>> CreateAsync(
            Guid userId,
            CreateProblemDto dto);

        Task<ApiResponse<bool>> UpdateAsync(
            Guid problemId,
            UpdateProblemDto dto);

        Task<ApiResponse<bool>> DeleteAsync(Guid problemId);

        Task<ProblemDetailsDto> GetDetailsAsync(Guid problemId);

        Task<List<ProblemDto>> GetContestProblemsAsync(Guid contestId);

        Task<List<ProblemDto>> GetTeacherProblemsAsync(Guid teacherId);

        Task<List<ProblemDto>> SearchAsync(string keyword);

        Task<List<PracticeProblemDto>> GetPracticeProblemsAsync(
            Guid studentId,
            Guid topicId,
            DifficultyType difficulty,
            int count);
    }
}
