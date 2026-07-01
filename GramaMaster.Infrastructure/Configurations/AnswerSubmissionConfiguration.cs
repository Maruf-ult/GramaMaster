using GramaMaster.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Infrastructure.Configurations
{
    public class AnswerSubmissionConfiguration:BaseEntityConfiguration<AnswerSubmission>
    {
        public override void Configure(EntityTypeBuilder<AnswerSubmission> builder)
        {
            base.Configure(builder);
            builder.ToTable("AnswerSubmissions");

            builder.HasOne(x => x.ExamAttempt)
                .WithMany(x => x.AnswerSubmissions)
                .HasForeignKey(x => x.ExamAttemptId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Problem)
                .WithMany(x => x.AnswerSubmissions)
                .HasForeignKey(x => x.ProblemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.StudentAnswer)
                .IsRequired();

            builder.Property(x => x.IsCorrect);

            builder.Property(x => x.AIExplanation);

        }
    }
}
