using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Domain.Entities
{
    public class ChatSession:BaseEntity
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public DateTime StartedAt { get; set; }
        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();

    }
}
