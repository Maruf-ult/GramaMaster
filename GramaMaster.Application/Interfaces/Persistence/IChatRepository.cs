using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Persistence
{
    public interface IChatRepository
    {
        Task<ChatSession?> GetSessionWithMessagesAsync(Guid sessionId);
        Task<List<ChatMessage>> GetHistoryAsync(Guid sessionId);
    }
}
