using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.DTOs.Topics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface ITopicService
    {
        Task<ApiResponse<TopicDto>> CreateTopicAsync(CreateTopicDto dto);

        Task<ApiResponse<bool>> UpdateTopicAsync(
            Guid topicId,
            UpdateTopicDto dto);

        Task<ApiResponse<bool>> DeleteTopicAsync(Guid topicId);

        Task<ApiResponse<List<TopicDto>>> GetAllTopicAsync();

        Task<ApiResponse<TopicDto>> GetTopicByIdAsync(Guid topicId);

        Task<ApiResponse<List<TopicDto>>> GetTopicByCurriculumAsync(Guid curriculumId);
    }
}
