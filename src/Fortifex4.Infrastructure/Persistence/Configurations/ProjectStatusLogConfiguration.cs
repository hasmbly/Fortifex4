using Fortifex4.Domain.Entities;
using Fortifex4.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fortifex4.Infrastructure.Persistence.Configurations
{
    public class ProjectStatusLogConfiguration : IEntityTypeConfiguration<ProjectStatusLog>
    {
        public void Configure(EntityTypeBuilder<ProjectStatusLog> builder)
        {
            builder.HasKey(e => e.ProjectStatusLogID);

            builder.Property(e => e.Comment).HasColumnType(SQLServerDataType.Varchar200);

            builder.HasOne(e => e.Project).WithMany(p => p.ProjectStatusLogs).HasForeignKey(e => e.ProjectID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}