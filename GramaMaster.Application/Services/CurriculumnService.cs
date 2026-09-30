using AutoMapper;
using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Curriculum;
using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Application.Interfaces.Services;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Services
{
    public class CurriculumnService:ICurriculumService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CurriculumnService(IMapper mapper,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ApiResponse<List<CurriculumDto>>> GetAllAsync()
        {
            var curriclums = await _unitOfWork.Curriculums.GetAllAsync();
            if (curriclums == null)
            {
                return ApiResponse<List<CurriculumDto>>.ErrorResponse(new List<String> { "No curriculums found" }, "Request Failed");
            }
            List<CurriculumDto> curr = _mapper.Map<List<CurriculumDto>>(curriclums);
            return ApiResponse<List<CurriculumDto>>.SuccessResponse(curr,"All Curriculumns fetched successfully");
        }

        public async Task<ApiResponse<CurriculumDto>> GetByIdAsync(Guid curriculumId)
        {
            var curr = await _unitOfWork.Curriculums.GetByIdAsync(curriculumId);

            if (curr == null)
            {
                return ApiResponse<CurriculumDto>.ErrorResponse(new[] { "Curriculmn not found" },"Request Failed");
            }
            CurriculumDto dto = _mapper.Map<CurriculumDto>(curr);
            return ApiResponse<CurriculumDto>.SuccessResponse(dto,"Curriculumn fetched successfully");
        }
    }
}
