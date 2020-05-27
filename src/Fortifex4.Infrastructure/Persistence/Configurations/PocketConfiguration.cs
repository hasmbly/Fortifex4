using Fortifex4.Domain.Entities;
using Fortifex4.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fortifex4.Infrastructure.Persistence.Configurations
{
    public class PocketConfiguration : IEntityTypeConfiguration<Pocket>
    {
        public void Configure(EntityTypeBuilder<Pocket> builder)
        {
            builder.HasKey(e => e.PocketID);

            builder.Property(e => e.Address).HasColumnType(SQLServerDataType.Varchar200).IsRequired();

            builder.HasOne(e => e.Wallet).WithMany(p => p.Pockets).HasForeignKey(e => e.WalletID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}