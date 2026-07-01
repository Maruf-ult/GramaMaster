
using GramaMaster.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GramaMaster.Infrastructure.Configurations
{
    public class TeamMembersConfiguration:BaseEntityConfiguration<TeamMember>
    {
        public override void Configure(EntityTypeBuilder<TeamMember> builder)
        {
            base.Configure(builder);
            builder.ToTable("TeamMembers");

            builder.HasIndex(x => new
            {
                x.TeamId,
                x.StudentId
            })
            .IsUnique();

            builder.HasOne(x => x.Team)
                .WithMany(x => x.TeamMembers)
                .HasForeignKey(x => x.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Student)
                .WithMany(x => x.TeamMembers)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.JoinedAt);


        }
    }
}
