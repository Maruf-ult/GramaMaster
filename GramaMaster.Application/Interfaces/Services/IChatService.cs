using GramaMaster.Application.DTOs.Chats;
using GramaMaster.Application.DTOs.Common;
using GramaMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.Interfaces.Services
{
    public interface IChatService
    {
        Task<ApiResponse<ChatResponseDto>> SendMessageAsync(
            Guid studentId,
            ChatRequestDto dto);

        Task<ApiResponse<List<ChatHistoryDto>>> GetHistoryAsync(Guid studentId);

        Task<ApiResponse<List<ChatMessageDto>>> GetSessionMessagesAsync(Guid sessionId);
    }
}
