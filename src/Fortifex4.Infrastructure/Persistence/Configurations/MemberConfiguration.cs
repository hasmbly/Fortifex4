using Fortifex4.Domain.Entities;
using Fortifex4.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fortifex4.Infrastructure.Persistence.Configurations
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(e => e.MemberUsername);

            builder.Property(e => e.MemberUsername).HasColumnType(SQLServerDataType.Varchar100);
            builder.Property(e => e.ExternalID).HasColumnType(SQLServerDataType.Varchar50);
            builder.Property(e => e.AuthenticationScheme).HasColumnType(SQLServerDataType.Varchar10).IsRequired();
            builder.Property(e => e.FirstName).HasColumnType(SQLServerDataType.Varchar50);
            builder.Property(e => e.LastName).HasColumnType(SQLServerDataType.Varchar50);
            builder.Property(e => e.PasswordHash).HasColumnType(SQLServerDataType.NVarchar200);
            builder.Property(e => e.PasswordSalt).HasColumnType(SQLServerDataType.NVarchar200);
            builder.Property(e => e.BirthDate).HasColumnType(SQLServerDataType.Date);
            builder.Property(e => e.PictureURL).HasColumnType(SQLServerDataType.Varchar2000);
            builder.Property(e => e.ActivationCode).HasColumnType(SQLServerDataType.UniqueIdentifier);


            builder.HasOne(t => t.PreferredFiatCurrency).WithMany().HasForeignKey(e => e.PreferredFiatCurrencyID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.PreferredCoinCurrency).WithMany().HasForeignKey(e => e.PreferredCoinCurrencyID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}