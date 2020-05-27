using Fortifex4.Domain.Entities;
using Fortifex4.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fortifex4.Infrastructure.Persistence.Configurations
{
    public class RegionConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.HasKey(e => e.RegionID);

            builder.Property(e => e.CountryCode).HasColumnType(SQLServerDataType.Varchar5).IsRequired();
            builder.Property(e => e.Name).HasColumnType(SQLServerDataType.Varchar100).IsRequired();

            builder.HasOne(e => e.Country).WithMany(p => p.Regions).HasForeignKey(e => e.CountryCode).OnDelete(DeleteBehavior.Restrict);
        }
    }
}