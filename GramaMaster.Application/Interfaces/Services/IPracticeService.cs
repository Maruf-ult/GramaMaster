using GramaMaster.Application.DTOs.Practice;
using GramaMaster.Application.DTOs.Problems;
using GramaMaster.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface IPracticeService
    {
        Task<ApiResponse<List<PracticeProblemDto>>> StartPracticeAsync( StartPracticeDto dto);
        Task<ApiResponse<PracticeResultDto>> SubmitPracticeAsync(Guid studentId,PracticeSubmissionDto dto);
        Task<ApiResponse<List<PracticeHistoryDto>>> GetHistoryAsync(Guid studentId);
    }
}
