using Fortifex4.Domain.Entities;
using Fortifex4.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fortifex4.Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(e => e.TransactionID);

            builder.Property(e => e.Amount).HasColumnType(SQLServerDataType.Decimal2910);
            builder.Property(e => e.UnitPriceInUSD).HasColumnType(SQLServerDataType.Decimal2920);
            builder.Property(e => e.TransactionHash).HasColumnType(SQLServerDataType.Varchar100);
            builder.Property(e => e.PairWalletName).HasColumnType(SQLServerDataType.Varchar200);
            builder.Property(e => e.PairWalletAddress).HasColumnType(SQLServerDataType.Varchar200);
            builder.Property(e => e.TransactionDateTime).HasColumnType(SQLServerDataType.DateTimeOffset);

            builder.HasOne(e => e.Pocket).WithMany(p => p.Transactions).HasForeignKey(e => e.PocketID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}