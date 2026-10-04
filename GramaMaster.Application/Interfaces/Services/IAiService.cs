using GramaMaster.Application.DTOs.AI;
using GramaMaster.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface IAIService
    {
        Task<ApiResponse<List<AIQuestionDto>>> GenerateQuestionsAsync(
            GenerateQuestionsDto dto);

        Task<ApiResponse<AIExplanationDto>> ExplainAnswerAsync(
            Guid answerSubmissionId);

        Task<ApiResponse<List<TopicRecommendationDto>>> RecommendTopicsAsync(
            Guid studentId);

        Task<ApiResponse<List<WeakTopicDto>>> AnalyzeWeakTopicsAsync(
            Guid studentId);
    }
}
