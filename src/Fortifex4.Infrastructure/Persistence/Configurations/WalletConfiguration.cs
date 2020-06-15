using Fortifex4.Domain.Entities;
using Fortifex4.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fortifex4.Infrastructure.Persistence.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.HasKey(e => e.WalletID);

            builder.Property(e => e.Name).HasColumnType(SQLServerDataType.Varchar100).IsRequired();
            builder.Property(e => e.Address).HasColumnType(SQLServerDataType.Varchar200);

            builder.HasOne(e => e.Owner).WithMany(p => p.Wallets).HasForeignKey(e => e.OwnerID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Blockchain).WithMany(p => p.Wallets).HasForeignKey(e => e.BlockchainID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}