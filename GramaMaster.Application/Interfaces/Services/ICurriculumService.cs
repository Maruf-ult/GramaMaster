using GramaMaster.Application.DTOs.Curriculum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface ICurriculumService
    {
        Task<List<CurriculumDto>> GetAllAsync();

        Task<CurriculumDto> GetByIdAsync(Guid curriculumId);
    }
}
