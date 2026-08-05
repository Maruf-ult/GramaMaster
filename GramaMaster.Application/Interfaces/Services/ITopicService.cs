using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Topics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface ITopicService
    {
        Task<ApiResponse<TopicDto>> CreateAsync(CreateTopicDto dto);

        Task<ApiResponse<bool>> UpdateAsync(
            Guid topicId,
            UpdateTopicDto dto);

        Task<ApiResponse<bool>> DeleteAsync(Guid topicId);

        Task<List<TopicDto>> GetAllAsync();

        Task<TopicDto> GetByIdAsync(Guid topicId);

        Task<List<TopicDto>> GetByCurriculumAsync(Guid curriculumId);
    }
}
