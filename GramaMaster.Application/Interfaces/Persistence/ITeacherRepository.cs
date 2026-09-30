using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Persistence
{
    public interface ITeacherRepository:IGenericRepository<Teacher>
    {
        Task<Teacher?> GetTeacherWithDetailsAsync(Guid teacherIdOrUserId);
        Task<int> GetTeacherContestCountAsync(Guid teacherId);
        Task<int> GetTotalProblemsCreatedAsync(Guid teacherId);
        Task<List<Problem>> GetProblemsCreatedAsync(Guid teacherId);
        Task<int> GetRunningContestCountAsync(Guid teacherId);
        Task<int> GetCompletedContestCountAsync(Guid teacherId);
        Task<List<Team>> GetTeamsByTeacherId(Guid teacherId);
        Task<bool> RemoveStudentByIdAsync(Guid studentId);
        Task<bool> DeleteTopicAsync(Guid topicId);
        Task<List<Topic>> GetTopicByCurriculumAsync(Guid curriculumId);
        Task<bool> DeleteGramaRuleAsync(Guid id);


    }
}
