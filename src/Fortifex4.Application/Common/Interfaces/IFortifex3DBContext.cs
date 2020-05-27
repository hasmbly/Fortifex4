using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Common.Interfaces
{
    public interface IFortifex4DBContext
    {
        DbSet<Blockchain> Blockchains { get; set; }
        DbSet<Country> Countries { get; set; }
        DbSet<Contributor> Contributors { get; set; }
        DbSet<Currency> Currencies { get; set; }
        DbSet<Gender> Genders { get; set; }
        DbSet<InternalTransfer> InternalTransfers { get; set; }
        DbSet<Member> Members { get; set; }
        DbSet<Owner> Owners { get; set; }
        DbSet<Pocket> Pockets { get; set; }
        DbSet<Provider> Providers { get; set; }
        DbSet<Region> Regions { get; set; }
        DbSet<TimeFrame> TimeFrames { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<Trade> Trades { get; set; }
        DbSet<Transaction> Transactions { get; set; }
        DbSet<Wallet> Wallets { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
    }
}