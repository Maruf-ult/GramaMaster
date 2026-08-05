using GramaMaster.Application.DTOs.Chats;
using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface IChatService
    {
        Task<ChatResponseDto> SendMessageAsync(
            Guid studentId,
            ChatRequestDto dto);

        Task<List<ChatHistoryDto>> GetHistoryAsync(Guid studentId);

        Task<List<ChatMessageDto>> GetSessionMessagesAsync(Guid sessionId);
    }
}
