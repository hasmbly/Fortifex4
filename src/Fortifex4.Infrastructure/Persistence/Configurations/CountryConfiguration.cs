using Fortifex4.Domain.Entities;
using Fortifex4.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fortifex4.Infrastructure.Persistence.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(e => e.CountryCode);

            builder.Property(e => e.CountryCode).HasColumnType(SQLServerDataType.Varchar5);
            builder.Property(e => e.Name).HasColumnType(SQLServerDataType.Varchar100).IsRequired();
        }
    }
}