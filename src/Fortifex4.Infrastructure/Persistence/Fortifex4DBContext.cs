using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Common;
using Fortifex4.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Infrastructure.Persistence
{
    public class Fortifex4DBContext : DbContext, IFortifex4DBContext
    {
        private readonly IDateTimeOffsetService _dateTimeOffset;

        public DbSet<Blockchain> Blockchains { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Contributor> Contributors { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<InternalTransfer> InternalTransfers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Pocket> Pockets { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<TimeFrame> TimeFrames { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        public Fortifex4DBContext(DbContextOptions<Fortifex4DBContext> options)
            : base(options)
        {
        }

        public Fortifex4DBContext(
            DbContextOptions<Fortifex4DBContext> options,
            IDateTimeOffsetService dateTimeOffset)
            : base(options)
        {
            _dateTimeOffset = dateTimeOffset;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = _dateTimeOffset.Now;
                        entry.Entity.LastModified = _dateTimeOffset.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTimeOffset.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = _dateTimeOffset.Now;
                        entry.Entity.LastModified = _dateTimeOffset.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTimeOffset.Now;
                        break;
                }
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Fortifex4DBContext).Assembly);
        }
    }
}