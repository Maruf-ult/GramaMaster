using GramaMaster.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Infrastructure.Configurations
{
    public class TeacherConfiguration:BaseEntityConfiguration<Teacher>
    {
        public override void Configure(EntityTypeBuilder<Teacher> builder)
        {
            base.Configure(builder);
            builder.ToTable("Teacher");

            builder.HasOne(x => x.User)
              .WithOne(x => x.Teacher)
              .HasForeignKey<Teacher>(x => x.UserId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.ProfileImageUrl)
              .HasMaxLength(1000);

            builder.Property(x => x.SscGroup)
             .IsRequired()
             .HasConversion<string>();

            builder.Property(x => x.SscBoard)
             .IsRequired()
             .HasConversion<string>();

            builder.Property(x => x.SscResult)
                .IsRequired();

            builder.Property(x => x.HscGroup)
             .IsRequired()
             .HasConversion<string>();

            builder.Property(x => x.HscBoard)
             .IsRequired()
             .HasConversion<string>();

            builder.Property(x => x.HscResult)
                .IsRequired();

            builder.Property(x => x.Department)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.University)
                .IsRequired()
                .HasMaxLength(1000);

            builder.HasMany(x => x.Teams)
                .WithOne(x => x.Teacher)
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);




        }
    }
}
