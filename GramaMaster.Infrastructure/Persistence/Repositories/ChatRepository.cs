using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Infrastructure.Persistence.Repositories
{
    public class ChatRepository:IChatRepository
    {
        public async Task<ChatSession?> GetSessionWithMessagesAsync(Guid sessionId)
        {

        }
        public async Task<List<ChatMessage>> GetHistoryAsync(Guid sessionId)
        {

        }
    }
}
