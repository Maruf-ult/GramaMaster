using GramaMaster.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Infrastructure.Configurations
{
    public class ChatConfiguration:BaseEntityConfiguration<ChatMessage>
    {
        public override void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            base.Configure(builder);
            builder.ToTable("ChatMessages");

            builder.HasOne(x => x.ChatSession)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.ChatSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Sender)
                .IsRequired();

            builder.Property(x => x.Content)
                .IsRequired();

            builder.Property(x => x.CreatedAt);
        }

    }
}
