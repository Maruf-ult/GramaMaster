using GramaMaster.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GramaMaster.Infrastructure.Configurations
{
    public class TeamConfiguration:BaseEntityConfiguration<Team>
    {
        public override void Configure(EntityTypeBuilder<Team> builder)
        {
            base.Configure(builder);
            builder.ToTable("Teams");

            builder.HasOne(x => x.Teacher)
                .WithMany(x => x.Teams)
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.JoinCode)
                .HasMaxLength(10)
                .IsRequired();

            builder.HasMany(x => x.TeamMembers)
                .WithOne(x => x.Team)
                .HasForeignKey(x => x.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Contests)
                .WithOne(x => x.Team)
                .HasForeignKey(x => x.TeamId)
                .OnDelete(DeleteBehavior.Cascade);



        }
    }
}
