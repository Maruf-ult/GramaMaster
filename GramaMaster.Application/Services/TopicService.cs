using AutoMapper;
using FluentValidation;
using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Topics;
using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Application.Interfaces.Services;
using GramaMaster.Application.Validators.Topic;
using GramaMaster.Domain.Entities;
using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Services
{
    public class TopicService:ITopicService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateTopicDto> _createTopicValidator;
        private readonly IValidator<UpdateTopicDto> _updateTopicValidator;

        public TopicService(IUnitOfWork unitOfWork,IMapper mapper,IValidator<CreateTopicDto>createTopicValidator,IValidator<UpdateTopicDto>?updateTopicValidator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _createTopicValidator = createTopicValidator ?? new CreateTopicValidator();
            _updateTopicValidator = updateTopicValidator ?? new UpdateTopicValidator();
        }

        public async Task<ApiResponse<TopicDto>> CreateTopicAsync(CreateTopicDto dto)
        {
            if(dto == null)
            {
                return ApiResponse<TopicDto>.ErrorResponse(new[] { "Payload can't be empty" }, "Invalid Request");
            }
            var validationResult = await _createTopicValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage);
                return ApiResponse<TopicDto>.ErrorResponse(errors, "Validation Failed");
            }
            var curriculumn = await _unitOfWork.Curriculums.FirstOrDefaultAsync(x => x.Id == dto.CurriculumId);

            var topic = new Topic
            {
                Id = Guid.NewGuid(),
                CurriculumId = dto.CurriculumId,
                Name = dto.Name,
                Description = dto.Description
            };

            await _unitOfWork.Topics.AddAsync(topic);
            await _unitOfWork.SaveChangesAsync();

            var topicDto = _mapper.Map<TopicDto>(topic);

            return ApiResponse<TopicDto>.SuccessResponse(
                topicDto,
                "Topic created successfully");
        }

        public async Task<ApiResponse<bool>> UpdateTopicAsync( Guid topicId,UpdateTopicDto dto)
        {
            if(dto == null)
            {
                return ApiResponse<bool>.ErrorResponse(new[] { "Payload can't be empty" }, "Invalid Request");
            }
            var validationResult = await _updateTopicValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage);
                return ApiResponse<bool>.ErrorResponse(errors, "Validation Failed");
            }
            var topic = await _unitOfWork.Topics.GetByIdAsync(topicId);

            if(topic == null)
            {
                return ApiResponse<bool>.ErrorResponse(new List<string> {"Topic not found"},"Request Falied");
            }

            topic.Name = dto.Name;
            topic.Description = dto.Description;

             _unitOfWork.Topics.Update(topic);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Topic updated successfully");

        }

        public async Task<ApiResponse<bool>> DeleteTopicAsync(Guid topicId)
        {
            var topic = await _unitOfWork.Topics.GetByIdAsync(topicId);

            if (topic == null)
            {
                return ApiResponse<bool>.ErrorResponse(new List<string> { "Topic not found" }, "Request Falied");
            }
            var delete = await _unitOfWork.Teachers.DeleteTopicAsync(topicId);

            if(delete== false)
            {
                return ApiResponse<bool>.ErrorResponse(new List<string> { "An error occured" }, "Request Failed");
            }
            return ApiResponse<bool>.SuccessResponse(true, "Topic deleted successfully");
            
        }

        public async Task<ApiResponse<List<TopicDto>>> GetAllTopicAsync()
        {
            var topics = await _unitOfWork.Topics.GetAllAsync();

            if(topics == null)
            {
                return ApiResponse<List<TopicDto>>.ErrorResponse(new List<string> { "No topics available" }, "Not Found");
            }
            List<TopicDto> teamDtos = _mapper.Map <List<TopicDto>>(topics);

            return ApiResponse<List<TopicDto>>.SuccessResponse(teamDtos, "All topics fetched successfully");

        }

        public async Task<ApiResponse<TopicDto>> GetTopicByIdAsync(Guid topicId)
        {
            var topic = await _unitOfWork.Topics.FirstOrDefaultAsync(x => x.Id == topicId);
            if(topic == null)
            {
                return ApiResponse<TopicDto>.ErrorResponse(new List<string> { "Topic not found" }, "Request Failed");
            }
            TopicDto topicDto = _mapper.Map<TopicDto>(topic);
            return ApiResponse<TopicDto>.SuccessResponse(topicDto, "Topic fetched successfully");
        }

        public async Task<ApiResponse<List<TopicDto>>> GetTopicByCurriculumAsync(Guid curriculumId)
        {
            var topics = await _unitOfWork.Teachers.GetTopicByCurriculumAsync(curriculumId);

            if (topics == null)
            {
                return ApiResponse<List<TopicDto>>.ErrorResponse(new List<string> { "No topics found" }, "Not Found");
            }

            List<TopicDto> TopicDtos = _mapper.Map<List<TopicDto>>(topics);

            return ApiResponse<List<TopicDto>>.SuccessResponse(TopicDtos, "Topics fetched successfully");
           
        }
    }
}
