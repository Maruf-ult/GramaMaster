using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Domain.Entities
{
    public class ChatMessage:BaseEntity
    {
        public Guid ChatSessionId { get; set; }
        public ChatSession ChatSession { get; set; } = null!;
        public string Sender { get; set; } = null!;
        public string Content { get; set; } = null!;
        public new DateTime CreatedAt { get; set; }

    }
}
