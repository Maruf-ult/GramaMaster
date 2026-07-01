using GramaMaster.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Infrastructure.Configurations
{
    public class ContestConfiguration:BaseEntityConfiguration<Contest>
    {
        public override void Configure(EntityTypeBuilder<Contest> builder)
        {
            base.Configure(builder);
            builder.ToTable("Contests");

            builder.Property(x => x.Title)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(x => x.ContestType)
                .IsRequired()
                .HasConversion<string>();

            builder.HasOne(x => x.Curriculum)
                .WithMany(x => x.Contests)
                .HasForeignKey(x => x.CurriculumId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.CreatedContests)
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Team)
                .WithMany(x => x.Contests)
                .HasForeignKey(x => x.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.StartAt)
                .IsRequired();

            builder.Property(x => x.EndAt)
                .IsRequired();

            builder.Property(x => x.DurationMinutes)
                .IsRequired();

            builder.HasMany(x => x.Problems)
                .WithOne(x => x.Contest)
                .HasForeignKey(x => x.ContestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.ExamAttempts)
                .WithOne(x => x.Contest)
                .HasForeignKey(x => x.ContestId)
                .OnDelete(DeleteBehavior.Cascade);

                
        }
    }
}
