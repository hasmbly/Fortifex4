using Fortifex4.Domain.Entities;
using Fortifex4.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fortifex4.Infrastructure.Persistence.Configurations
{
    public class BlockchainConfiguration : IEntityTypeConfiguration<Blockchain>
    {
        public void Configure(EntityTypeBuilder<Blockchain> builder)
        {
            builder.HasKey(e => e.BlockchainID);

            builder.Property(e => e.BlockchainID).ValueGeneratedNever();
            builder.Property(e => e.Symbol).HasColumnType(SQLServerDataType.NVarchar200).IsRequired();
            builder.Property(e => e.Name).HasColumnType(SQLServerDataType.NVarchar200).IsRequired();
        }
    }
}