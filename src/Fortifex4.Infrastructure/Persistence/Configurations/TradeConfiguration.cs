using Fortifex4.Domain.Entities;
using Fortifex4.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fortifex4.Infrastructure.Persistence.Configurations
{
    public class TradeConfiguration : IEntityTypeConfiguration<Trade>
    {
        public void Configure(EntityTypeBuilder<Trade> builder)
        {
            builder.HasKey(e => e.TradeID);

            builder.Property(e => e.UnitPrice).HasColumnType(SQLServerDataType.Decimal2920);
            
            builder.HasOne(t => t.FromTransaction).WithMany(z => z.FromTrades).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.ToTransaction).WithMany(z => z.ToTrades).OnDelete(DeleteBehavior.Restrict);
        }
    }
}