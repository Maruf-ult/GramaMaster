using GramaMaster.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Infrastructure.Configurations
{
    public class CurriculumConfiguration:BaseEntityConfiguration<Curriculum>
    {
        public override void Configure(EntityTypeBuilder<Curriculum> builder)
        {
            base.Configure(builder);
            builder.ToTable("Curriculums");

            builder.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(1000)
                .IsRequired();

            builder.HasMany(x => x.Topics)
                .WithOne(x => x.Curriculum)
                .HasForeignKey(x => x.CurriculumId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Contests)
                .WithOne(x => x.Curriculum)
                .HasForeignKey(x => x.CurriculumId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Students)
                .WithOne(x => x.Curriculum)
                .HasForeignKey(x => x.CurriculumId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
