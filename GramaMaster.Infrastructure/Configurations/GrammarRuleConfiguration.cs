using GramaMaster.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Infrastructure.Configurations
{
    public class GrammarRuleConfiguration:BaseEntityConfiguration<GrammarRule>
    {

        public override void Configure(EntityTypeBuilder<GrammarRule> builder)
        {
            base.Configure(builder);
            builder.ToTable("GrammarRule");

            builder.HasOne(x => x.Topic)
                .WithMany(x => x.GrammerRules)
                .HasForeignKey(x => x.TopicId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Title)
                .HasMaxLength(150)
                .IsRequired();

            builder.HasIndex(x => x.Title);

            builder.Property(x => x.Content)
               .IsRequired();

            builder.Property(x => x.Example)
                .IsRequired();






        }
    }
}
