using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface IUnitOfWorkService
    {
        IGenericService<Student> Students { get; }

        IGenericService<Teacher> Teachers { get; }

        IGenericService<Team> Teams { get; }

        IGenericService<TeamMember> TeamMembers { get; }

        IGenericService<Topic> Topics { get; }

        IGenericService<Curriculum> Curriculums { get; }

        IGenericService<GrammarRule> GrammarRules { get; }

        IUserService Users { get; }

        IContestService Contests { get; }

        IProblemService Problems { get; }

        IExamService Exams { get; }

        IChatService Chats { get; }

        Task<int> SaveChangesAsync();
    }
}
