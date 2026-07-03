    using GramaMaster.Application.DTOs.Common;
    using GramaMaster.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Text;

    namespace GramaMaster.Application.Interfaces.Persistence
    {
        public interface IContestRepository
        {
            Task<List<Contest>> GetGlobalContestsAsync(QueryDto query);
            Task<List<Contest>> GetTeamContestsAsync(Guid teamId);
            Task<Contest?> GetContestWithProblemsAsync(Guid contestId);
            Task<bool> IsContestRunningAsync(Guid contestId);
            Task<List<Contest>> GetUpcomingContestsAsync(QueryDto query);
            Task<List<Contest>> GetRunningContestsAsync(QueryDto query);
            Task<List<Contest>> GetCompletedContestsAsync(QueryDto query);
            Task<Contest?> GetContestWithAttemptsAsync(Guid contestId);

        Task<List<Contest>> GetContestsByCurriculumAsync(Guid curriculumId);

        Task<List<Contest>> GetTeacherContestsAsync(Guid teacherUserId, QueryDto query);

        Task<List<Contest>> GetStudentAvailableContestsAsync(Guid studentId, QueryDto query);

        Task<bool> HasStudentJoinedAsync(Guid contestId, Guid studentId);

        Task<bool> HasStudentSubmittedAsync(Guid contestId, Guid studentId);

        Task<int> GetContestParticipantCountAsync(Guid contestId);
    }
    }
