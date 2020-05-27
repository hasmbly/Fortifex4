using Fortifex4.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fortifex4.Infrastructure.Persistence.Configurations
{
    public class InternalTransferConfiguration : IEntityTypeConfiguration<InternalTransfer>
    {
        public void Configure(EntityTypeBuilder<InternalTransfer> builder)
        {
            builder.HasKey(e => e.InternalTransferID);

            builder.HasOne(t => t.FromTransaction).WithMany(z => z.FromInternalTransfers).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.ToTransaction).WithMany(z => z.ToInternalTransfers).OnDelete(DeleteBehavior.Restrict);
        }
    }
}