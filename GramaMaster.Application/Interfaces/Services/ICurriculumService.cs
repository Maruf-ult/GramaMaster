using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Curriculum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface ICurriculumService
    {
        Task<ApiResponse<List<CurriculumDto>>> GetAllAsync();

        Task<ApiResponse<CurriculumDto>> GetByIdAsync(Guid curriculumId);
    }
}
