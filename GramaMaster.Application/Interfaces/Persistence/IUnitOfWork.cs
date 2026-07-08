using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Persistence
{
    public interface IUnitOfWork
    {
        IGenericRepository<Student> Students { get; }

        IGenericRepository<Teacher> Teachers { get; }

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
