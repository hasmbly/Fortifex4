using Fortifex4.Domain.Entities;
using Fortifex4.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fortifex4.Infrastructure.Persistence.Configurations
{
    public class GenderConfiguration : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.HasKey(e => e.GenderID);

            builder.Property(e => e.GenderID).ValueGeneratedNever();
            builder.Property(e => e.Name).HasColumnType(SQLServerDataType.Varchar10).IsRequired();
        }
    }
}