using GramaMaster.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Infrastructure.Configurations
{

    public class ChatSessionConfiguration:BaseEntityConfiguration<ChatSession>
    {
        public override void Configure(EntityTypeBuilder<ChatSession> builder)
        {
            base.Configure(builder);
            builder.ToTable("ChatSession");

            builder.HasOne(x => x.Student)
                .WithMany(x => x.ChatSessions)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.StartedAt)
                .HasConversion<string>();

            builder.HasMany(x => x.Messages)
                .WithOne(x => x.ChatSession)
                .HasForeignKey(x => x.ChatSessionId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
