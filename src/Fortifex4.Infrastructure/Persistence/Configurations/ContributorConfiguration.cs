using Fortifex4.Domain.Entities;
using Fortifex4.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fortifex4.Infrastructure.Persistence.Configurations
{
    public class ContributorConfiguration : IEntityTypeConfiguration<Contributor>
    {
        public void Configure(EntityTypeBuilder<Contributor> builder)
        {
            builder.HasKey(e => e.ContributorID);

            builder.Property(e => e.MemberUsername).HasColumnType(SQLServerDataType.Varchar100).IsRequired();
            builder.Property(e => e.InvitationCode).HasColumnType(SQLServerDataType.UniqueIdentifier);

            builder.HasOne(e => e.Project).WithMany(p => p.Contributors).HasForeignKey(e => e.ProjectID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}