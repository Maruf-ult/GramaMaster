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
        Task<int> GetRunningContestCountAsync(Guid teacherId);
        Task<int> GetCompletedContestCountAsync(Guid teacherId);
        
    }
}
