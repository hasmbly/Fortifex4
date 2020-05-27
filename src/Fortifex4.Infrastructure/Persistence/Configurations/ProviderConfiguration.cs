using Fortifex4.Domain.Entities;
using Fortifex4.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fortifex4.Infrastructure.Persistence.Configurations
{
    public class ProviderConfiguration : IEntityTypeConfiguration<Provider>
    {
        public void Configure(EntityTypeBuilder<Provider> builder)
        {
            builder.HasKey(e => e.ProviderID);

            builder.Property(e => e.ProviderID).ValueGeneratedNever();
            builder.Property(e => e.Name).HasColumnType(SQLServerDataType.Varchar100).IsRequired();
            builder.Property(e => e.SiteURL).HasColumnType(SQLServerDataType.Varchar2000);
        }
    }
}