using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.AI
{
    public class TopicRecommendationDto
    {
        public string Title { get; set; } = string.Empty;

        public string Recommendation { get; set; } = string.Empty;

        public List<WeakTopicDto> WeakTopics { get; set; } = new();
    }
}
