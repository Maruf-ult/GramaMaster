using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Chats
{
    public class ChatHistoryDto
    {
        public Guid SessionId { get; set; }

        public List<ChatMessageDto> Messages { get; set; } = new();
    }
}
