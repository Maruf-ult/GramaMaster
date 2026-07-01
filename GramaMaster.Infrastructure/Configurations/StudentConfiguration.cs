using GramaMaster.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Infrastructure.Configurations
{
    public class StudentConfiguration:BaseEntityConfiguration<Student>
    {
        public override void Configure(EntityTypeBuilder<Student> builder)
        {

            base.Configure(builder);
            builder.ToTable("Students");


            builder.HasOne(x => x.User)
                .WithOne(x => x.Student)
                .HasForeignKey<Student>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.ProfileImageUrl)
                .HasMaxLength(1000);

            builder.Property(x => x.InstitutionName)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(x => x.Curriculum)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.Group)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.ExamBatchYear)
                .IsRequired();

            builder.Property(x => x.Board)
                .IsRequired()
                .HasConversion<string>();

            builder.HasMany(x => x.TeamMembers)
                .WithOne(x => x.Student)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.ExamAttempts)
               .WithOne(x => x.Student)
               .HasForeignKey(x => x.StudentId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.ChatSessions)
               .WithOne(x => x.Student)
               .HasForeignKey(x => x.StudentId)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
