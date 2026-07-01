using GramaMaster.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GramaMaster.Infrastructure.Data
{
    public class GramaMasterDbContext:DbContext
    {
        public GramaMasterDbContext(DbContextOptions<GramaMasterDbContext> options):base(options)
        {

        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<Topic> Topics => Set<Topic>();
        public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
        public DbSet<Curriculum> Curriculums => Set<Curriculum>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<ProblemOptions> ProblemOptions => Set<ProblemOptions>();
        public DbSet<Problem> Problems => Set<Problem>();
        public DbSet<GrammarRule> GrammarRules => Set<GrammarRule>();
        public DbSet<ExamAttempt> ExamAttempts => Set<ExamAttempt>();
        public DbSet<Contest> Contests => Set<Contest>();
        public DbSet<ChatSession> ChatSessions => Set<ChatSession>();
        public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
        public DbSet<AnswerSubmission> AnswerSubmissions => Set<AnswerSubmission>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly()
                );
        }


    }
}
