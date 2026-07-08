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
    }
}
