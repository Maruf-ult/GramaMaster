using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Persistence
{
    public interface IStudentRepository:IGenericRepository<Student>
    {
        Task<Student?> GetStudentWithDetailsAsync(Guid studentIdOrUserId);
        Task<List<ExamAttempt>> GetStudentAttemptsWithAnswersAsync(Guid studentId);
        Task<List<Topic>> GetCurriculumTopicsWithProblemsAsync(Guid curriculumId);
        Task<List<Team>> GetTeamsByStudentIdAsync(Guid studentId);
        Task<Team?> GetByJoinCodeAsync(string joinCode);
        Task<List<ExamAttempt>> GetStudentHistory(Guid studentId);
    }
}
