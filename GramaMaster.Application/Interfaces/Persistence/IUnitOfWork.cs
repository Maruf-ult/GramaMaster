using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Persistence
{
    public interface IUnitOfWork
    {
        IStudentRepository Students { get; }

        ITeacherRepository Teachers { get; }

        IGenericRepository<Team> Teams { get; }

        IGenericRepository<TeamMember> TeamMembers { get; }

        IGenericRepository<Topic> Topics { get; }

        IGenericRepository<Curriculum> Curriculums { get; }

        IGenericRepository<GrammarRule> GrammarRules { get; }

        IUserRepository Users { get; }

        IContestRepository Contests { get; }

        IProblemRepository Problems { get; }

        IExamRepository Exams { get; }

        IChatRepository Chats { get; }

        Task<int> SaveChangesAsync();
    }
}
