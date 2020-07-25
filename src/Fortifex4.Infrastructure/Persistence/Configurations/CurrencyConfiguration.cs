using Fortifex4.Domain.Entities;
using Fortifex4.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fortifex4.Infrastructure.Persistence.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.HasKey(e => e.CurrencyID);

            builder.Property(e => e.Symbol).HasColumnType(SQLServerDataType.NVarchar200).IsRequired();
            builder.Property(e => e.Name).HasColumnType(SQLServerDataType.NVarchar200).IsRequired();
            builder.Property(e => e.UnitPriceInUSD).HasColumnType(SQLServerDataType.Decimal2920).IsRequired(); //CMC setidaknya butuh 5 digit di depan koma. Kita jaga-jaga ada 9 digit.
            builder.Property(e => e.Volume24h).HasColumnType(SQLServerDataType.Decimal2915).IsRequired(); //CMC setidaknya butuh 11 digit di depan koma. Kita jaga-jaga ada 14 digit.
            builder.Property(e => e.PercentChange1h).HasColumnType(SQLServerDataType.Real).IsRequired();
            builder.Property(e => e.PercentChange24h).HasColumnType(SQLServerDataType.Real).IsRequired();
            builder.Property(e => e.PercentChange7d).HasColumnType(SQLServerDataType.Real).IsRequired();
            builder.Property(e => e.LastUpdated).HasColumnType(SQLServerDataType.DateTimeOffset).IsRequired();

            builder.HasOne(e => e.Blockchain).WithMany(p => p.Currencies).HasForeignKey(e => e.BlockchainID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}