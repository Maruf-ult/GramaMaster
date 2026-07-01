using GramaMaster.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Infrastructure.Configurations
{
    public class ProblemConfiguration:BaseEntityConfiguration<Problem>
    {
        public override void Configure(EntityTypeBuilder<Problem> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.Topic)
                .WithMany(x => x.Problems)
                .HasForeignKey(x => x.TopicId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Contest)
                .WithMany(x => x.Problems)
                .HasForeignKey(x => x.ContestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.CreatedProblems)
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.ProblemType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.QuestionText)
                .IsRequired();


            builder.Property(x => x.CorrectAns)
                .IsRequired();

            builder.Property(x => x.Explanation)
                .IsRequired();

            builder.Property(x => x.Difficulty)
                .IsRequired()
                .HasConversion<string>();


            builder.Property(x => x.IsAiGenerated)
                .IsRequired();

            builder.HasMany(x => x.ProblemOptions)
                .WithOne(x => x.Problem)
                .HasForeignKey(x => x.ProblemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.ProblemOptions)
                .WithOne(x => x.Problem)
                .HasForeignKey(x => x.ProblemId)
                .OnDelete(DeleteBehavior.Cascade);





        }
    }
}
