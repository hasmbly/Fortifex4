using Fortifex4.Domain.Entities;
using Fortifex4.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fortifex4.Infrastructure.Persistence.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(e => e.ProjectID);

            builder.Property(e => e.MemberUsername).HasColumnType(SQLServerDataType.Varchar100).IsRequired();
            builder.Property(e => e.Name).HasColumnType(SQLServerDataType.Varchar200).IsRequired();
            builder.Property(e => e.Description).HasColumnType(SQLServerDataType.Varchar500);
            builder.Property(e => e.WalletAddress).HasColumnType(SQLServerDataType.Varchar200).IsRequired();

            builder.HasOne(e => e.Member).WithMany(p => p.Projects).HasForeignKey(e => e.MemberUsername).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Blockchain).WithMany(p => p.Projects).HasForeignKey(e => e.BlockchainID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}