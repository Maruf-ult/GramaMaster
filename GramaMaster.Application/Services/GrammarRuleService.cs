using AutoMapper;
using FluentValidation;
using GramaMaster.Application.Common.Mappings;
using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.GrammerRules;
using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Application.Interfaces.Services;
using GramaMaster.Application.Validators.GrammerRules;
using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GramaMaster.Application.Services
{
    public class GrammarRuleService:IGrammarRuleService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateGrammarRuleDto> _createGrammarRuleValidator;
        private readonly IValidator<UpdateGrammarRuleDto> _updateGrammarRuleValidator;

        public GrammarRuleService(IMapper mapper,IUnitOfWork unitOfWork,IValidator<CreateGrammarRuleDto> createGrammarRuleValidator,IValidator<UpdateGrammarRuleDto>?updateGrammarRuleValidator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _createGrammarRuleValidator = createGrammarRuleValidator ?? new CreateGrammarRuleValidator();
            _updateGrammarRuleValidator = updateGrammarRuleValidator ?? new UpdateGrammarRuleValidator();

        }


        public async Task<ApiResponse<GrammarRuleDto>> CreateAsync(CreateGrammarRuleDto dto)
        {
            if(dto == null)
            {
                return ApiResponse<GrammarRuleDto>.ErrorResponse(new[] {"Payload cant be null" }, "Request Failed");
            }
            var validationResult = await _createGrammarRuleValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage);
                return ApiResponse<GrammarRuleDto>.ErrorResponse(errors, "Validation Failed");
            }
            var topic = await _unitOfWork.Topics.GetByIdAsync(dto.TopicId);

            var newGrammerRule = new GrammarRule
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Content = dto.Content,
                Example = dto.Example,
                TopicId = dto.TopicId,
                Topic = topic,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.SaveChangesAsync();
            GrammarRuleDto grama = _mapper.Map<GrammarRuleDto>(newGrammerRule);
            return ApiResponse<GrammarRuleDto>.SuccessResponse(grama, "Grammer Rule created successfully");
        }

        public async Task<ApiResponse<bool>> UpdateAsync(Guid id,UpdateGrammarRuleDto dto)
        {
            var grama = await _unitOfWork.GrammarRules.GetByIdAsync(id);
            if(grama == null)
            {
                return ApiResponse<bool>.ErrorResponse(new[] { "gramarules not found" }, "Request Failed");
            }
            grama.Title = dto.Title;
            grama.Content = dto.Content;
            grama.Example = dto.Example;

            grama.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Grammer rules updated successfully");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var grama = await _unitOfWork.GrammarRules.GetByIdAsync(id);
            if (grama == null)
            {
                return ApiResponse<bool>.ErrorResponse(new[] { "gramarules not found" }, "Request Failed");
            }
            var delete =await _unitOfWork.Teachers.DeleteGramaRuleAsync(id);

            if(delete == false)
            {
                return ApiResponse<bool>.ErrorResponse(new[] {"An error occured"},"Request Failed");
            }
            return ApiResponse<bool>.SuccessResponse(true, "Grammar rule deleted successfully");
        }

        public async Task<ApiResponse<GrammarRuleDto>> GetTopicRulesAsync(Guid topicId)
        {
            var gramaRules = await _unitOfWork.GrammarRules.FirstOrDefaultAsync(x => x.TopicId == topicId && !x.IsDeleted);

           
            if(gramaRules == null)
            {
                return ApiResponse<GrammarRuleDto>.ErrorResponse(new List<string> { "No Topic Rules found" }, "Request Failed");
            }
            GrammarRuleDto grama = _mapper.Map<GrammarRuleDto>(gramaRules);
         
            return ApiResponse<GrammarRuleDto>.SuccessResponse(grama, "Topic rules fetched successfully");
            

        }

        public async Task<ApiResponse<GrammarRuleDto>> GetByIdAsync(Guid id)
        {
            var grama = await _unitOfWork.GrammarRules.GetByIdAsync(id);
            if (grama == null)
            {
                return ApiResponse<GrammarRuleDto>.ErrorResponse(new[] { "gramarules not found" }, "Request Failed");
            }
            GrammarRuleDto go = _mapper.Map<GrammarRuleDto>(grama);
            return ApiResponse<GrammarRuleDto>.SuccessResponse(go, "Grammer rule fetched successfully");
        }
    }
}
