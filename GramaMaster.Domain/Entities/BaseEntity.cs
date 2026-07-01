using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] RowVersion { get; set; } = null!;
    }
}
