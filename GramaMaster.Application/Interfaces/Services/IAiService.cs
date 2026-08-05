using GramaMaster.Application.DTOs.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface IAIService
    {
        Task<List<AIQuestionDto>> GenerateQuestionsAsync(
            GenerateQuestionsDto dto);

        Task<AIExplanationDto> ExplainAnswerAsync(
            Guid answerSubmissionId);

        Task<List<TopicRecommendationDto>> RecommendTopicsAsync(
            Guid studentId);

        Task<List<WeakTopicDto>> AnalyzeWeakTopicsAsync(
            Guid studentId);
    }
}
