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
        Task<ApiResponse<ProblemDto>> CreateProblemAsync( Guid userId,CreateProblemDto dto);

        Task<ApiResponse<bool>> UpdateProblemAsync(Guid problemId,UpdateProblemDto dto);

        Task<ApiResponse<bool>> DeleteProblemAsync(Guid problemId);

        Task<ApiResponse<ProblemDetailsDto>> GetProblemDetailsAsync(Guid problemId);

        Task<ApiResponse<List<ProblemDto>>> GetContestProblemsAsync(Guid contestId);

        Task<ApiResponse<List<ProblemDto>>> GetTeacherProblemsAsync(Guid teacherId);

        Task<ApiResponse<List<ProblemDto>>> SearchProblemAsync(string keyword);

        Task<ApiResponse<List<PracticeProblemDto>>> GetPracticeProblemsAsync(Guid studentId,Guid topicId,DifficultyType difficulty, int count);
    }
}
