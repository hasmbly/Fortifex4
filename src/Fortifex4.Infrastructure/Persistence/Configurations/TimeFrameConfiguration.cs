using Fortifex4.Domain.Entities;
using Fortifex4.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Fortifex4.Infrastructure.Persistence.Configurations
{
    public class TimeFrameConfiguration : IEntityTypeConfiguration<TimeFrame>
    {
        public void Configure(EntityTypeBuilder<TimeFrame> builder)
        {
            builder.HasKey(e => e.TimeFrameID);

            builder.Property(e => e.TimeFrameID).ValueGeneratedNever();
            builder.Property(e => e.Name).HasColumnType(SQLServerDataType.Varchar10).IsRequired();
        }
    }
}