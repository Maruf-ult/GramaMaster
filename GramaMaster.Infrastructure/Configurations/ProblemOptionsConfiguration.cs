using GramaMaster.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Infrastructure.Configurations
{
    public class ProblemOptionsConfiguration:BaseEntityConfiguration<ProblemOptions>
    {

        public override void Configure(EntityTypeBuilder<ProblemOptions> builder)
        {
            base.Configure(builder);
            builder.ToTable("ProblemOptions");

            builder.HasOne(x => x.Problem)
                .WithMany(x => x.ProblemOptions)
                .HasForeignKey(x => x.ProblemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.OptionText)
                .HasMaxLength(200);

            builder.Property(x => x.IsCorrect);

        }
    }
}
