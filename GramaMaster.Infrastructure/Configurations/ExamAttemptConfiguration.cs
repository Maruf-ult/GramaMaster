using GramaMaster.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Infrastructure.Configurations
{
    public class ExamAttemptConfiguration:BaseEntityConfiguration<ExamAttempt>
    {

        public override void Configure(EntityTypeBuilder<ExamAttempt> builder)
        {
            base.Configure(builder);

            builder.ToTable("ExamAttempts");

            builder.HasOne(x => x.Student)
                .WithMany(x => x.ExamAttempts)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Contest)
               .WithMany(x => x.ExamAttempts)
               .HasForeignKey(x => x.ContestId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.StartedAt);

            builder.Property(x => x.SubmittedAt);

            builder.Property(x => x.Score);

            builder.Property(x => x.TotalQuestions);

            builder.HasMany(x => x.AnswerSubmissions)
                .WithOne(x => x.ExamAttempt)
                .HasForeignKey(x => x.ExamAttemptId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.ContestId, x.Score });





        }
    }
}
