using GramaMaster.Domain.Entities;
using GramaMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Persistence
{
    public interface IProblemRepository
    {
        Task<List<Problem>> GetByTopicAsync(Guid topicId);
        Task<List<Problem>> GetPracticeProblemsAsync(
            Guid curriculumId,
            Guid topicId,
            DifficultyType difficulty);
        Task<List<Problem>> GetContestProblemsAsync(Guid contestId);
    }
}
