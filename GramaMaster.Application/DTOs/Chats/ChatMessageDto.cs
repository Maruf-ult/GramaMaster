using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Application.DTOs.Chats
{
    public class ChatMessageDto
    {
        public string Sender { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
