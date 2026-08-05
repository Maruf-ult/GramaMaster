using GramaMaster.Application.DTOs.Practice;
using GramaMaster.Application.DTOs.Problems;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface IPracticeService
    {
        Task<List<PracticeProblemDto>> StartPracticeAsync(
            StartPracticeDto dto);

        Task<PracticeResultDto> SubmitPracticeAsync(
            Guid studentId,
            PracticeSubmissionDto dto);

        Task<List<PracticeHistoryDto>> GetHistoryAsync(Guid studentId);
    }
}
