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
            DifficultyType difficulty,
            int count);

        Task<List<Problem>> GetRandomPracticeProblemsAsync(
            Guid curriculumId,
            Guid topicId,
            DifficultyType difficulty,
            int count);

        Task<List<Problem>> GetContestProblemsAsync(Guid contestId);

        Task<Problem?> GetProblemWithOptionsAsync(Guid problemId);

        Task<List<Problem>> GetTeacherProblemsAsync(Guid teacherUserId);

        Task<List<Problem>> SearchProblemsAsync(string keyword);

        Task<int> GetProblemCountByTopicAsync(Guid topicId);

        Task<int> GetPracticeProbCountByStudentIdAsync(Guid studentId);
        Task<int> GetOverallAccuracyByStudentIdAsync(Guid studentId);
    }
}
