using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Domain.Entities;
using GramaMaster.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Infrastructure.Persistence.Repositories
{
    public class ChatRepository:GenericRepository<ChatMessage>,IChatRepository
    {
        private readonly GramaMasterDbContext _dbContext;

        public ChatRepository(GramaMasterDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ChatSession?> GetSessionWithMessagesAsync(Guid sessionId)
        {
            return await _dbContext.ChatSessions
                .Include(x => x.Messages)
                .FirstOrDefaultAsync(x => x.Id == sessionId && !x.IsDeleted);
        }
        public async Task<List<ChatMessage>> GetHistoryAsync(Guid sessionId)
        {
            return await _dbContext.ChatMessages
                     .Where(x => x.ChatSessionId == sessionId && !x.IsDeleted)
                     .OrderBy(x => x.CreatedAt)
                     .ToListAsync();
        }

        public async Task<List<ChatSession>> GetStudentSessionsAsync(Guid studentId)
        {
            return await _dbContext.ChatSessions
                .Include(x => x.Messages.Where(m => !m.IsDeleted).OrderBy(m => m.CreatedAt))
                .Where(x => x.StudentId == studentId && !x.IsDeleted)
                .OrderByDescending(x => x.StartedAt)
                .ToListAsync();
        }

        public async Task AddSessionAsync(ChatSession session)
        {
            session.CreatedAt = DateTime.UtcNow;
            await _dbContext.ChatSessions.AddAsync(session);
        }
    }
}
