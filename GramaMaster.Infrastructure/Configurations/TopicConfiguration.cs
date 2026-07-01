using GramaMaster.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Infrastructure.Configurations
{
    public class TopicConfiguration:BaseEntityConfiguration<Topic>
    {
        public override void Configure(EntityTypeBuilder<Topic> builder)
        {
            base.Configure(builder);
            builder.ToTable("Topics");

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.HasIndex(x => x.Name);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.HasOne(x => x.Curriculum)
                .WithMany(x => x.Topics)
                .HasForeignKey(x => x.CurriculumId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Problems)
                .WithOne(x => x.Topic)
                .HasForeignKey(x => x.TopicId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.GrammerRules)
                 .WithOne(x => x.Topic)
                 .HasForeignKey(x => x.TopicId)
                 .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
