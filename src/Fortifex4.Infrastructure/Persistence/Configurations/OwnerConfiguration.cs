using Fortifex4.Domain.Entities;
using Fortifex4.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fortifex4.Infrastructure.Persistence.Configurations
{
    public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.HasKey(e => e.OwnerID);

            builder.Property(e => e.MemberUsername).HasColumnType(SQLServerDataType.Varchar100).IsRequired();

            builder.HasOne(e => e.Member).WithMany(p => p.Owners).HasForeignKey(e => e.MemberUsername).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Provider).WithMany(p => p.Owners).HasForeignKey(e => e.ProviderID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}