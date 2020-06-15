using Fortifex4.Domain.Entities;
using Fortifex4.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fortifex4.Infrastructure.Persistence.Configurations
{
    public class ProjectDocumentConfiguration : IEntityTypeConfiguration<ProjectDocument>
    {
        public void Configure(EntityTypeBuilder<ProjectDocument> builder)
        {
            builder.HasKey(e => e.ProjectDocumentID);

            builder.Property(e => e.Title).HasColumnType(SQLServerDataType.Varchar100).IsRequired();
            builder.Property(e => e.DocumentID).HasColumnType(SQLServerDataType.UniqueIdentifier).IsRequired();
            builder.Property(e => e.OriginalFileName).HasColumnType(SQLServerDataType.Varchar100).IsRequired();

            builder.HasOne(e => e.Project).WithMany(p => p.ProjectDocuments).HasForeignKey(e => e.ProjectID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}