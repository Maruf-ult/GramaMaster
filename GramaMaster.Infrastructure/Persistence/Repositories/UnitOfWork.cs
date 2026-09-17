using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Domain.Entities;
using GramaMaster.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly GramaMasterDbContext _dbContext;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(GramaMasterDbContext dbContext)
        {
            _dbContext = dbContext;

            Students = new StudentRepository(_dbContext);
            Teachers = new TeacherRepository(_dbContext);
            Teams = new GenericRepository<Team>(_dbContext);
            TeamMembers = new GenericRepository<TeamMember>(_dbContext);
            Topics = new GenericRepository<Topic>(_dbContext);
            Curriculums = new GenericRepository<Curriculum>(_dbContext);
            GrammarRules = new GenericRepository<GrammarRule>(_dbContext);

            Users = new UserRepository(_dbContext);
            Contests = new ContestRepository(_dbContext);
            Problems = new ProblemRepository(_dbContext);
            Exams = new ExamRepository(_dbContext);
            Chats = new ChatRepository(_dbContext);
        }


        public IStudentRepository Students { get; }

        public ITeacherRepository Teachers { get; }

        public IGenericRepository<Team> Teams { get; }

        public IGenericRepository<TeamMember> TeamMembers { get; }

        public IGenericRepository<Topic> Topics { get; }

        public IGenericRepository<Curriculum> Curriculums { get; }

        public IGenericRepository<GrammarRule> GrammarRules { get; }


        public IUserRepository Users { get; }

        public IContestRepository Contests { get; }

        public IProblemRepository Problems { get; }

        public IExamRepository Exams { get; }

        public IChatRepository Chats { get; }


        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _dbContext.Database.BeginTransactionAsync();
            }
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

    }
    }

