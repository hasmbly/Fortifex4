using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Fortifex4.Application.Sync.Queries.GetSync
{
    public class GetSyncQueryHandler : IRequestHandler<GetSyncQuery, GetSyncResult>
    {
        private readonly IFortifex4DBContext _context;

        public GetSyncQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }
        public async Task<GetSyncResult> Handle(GetSyncQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions
                .Where(x => x.TransactionID == request.TransactionID)
                .Include(a => a.Pocket)
                    .ThenInclude(b => b.Wallet)
                .Include(a => a.Pocket)
                    .ThenInclude(b => b.Currency)
                .SingleOrDefaultAsync(cancellationToken);

            if (transaction == null)
                throw new NotFoundException(nameof(Transaction), request.TransactionID);

            if (transaction.TransactionType == TransactionType.SyncBalanceImport)
            {
                var result = new GetSyncResult
                {
                    WalletName = transaction.Pocket.Wallet.Name,
                    WalletMainPocketCurrencyName = transaction.Pocket.Currency.Name,
                    TransactionType = transaction.TransactionType,
                    Amount = transaction.Amount,
                    TransactionDateTime = transaction.TransactionDateTime,
                    UnitPriceInUSD = transaction.UnitPriceInUSD,
                    PairWalletName = transaction.PairWalletName,
                    PairWalletAddress = transaction.PairWalletAddress
                };

                return result;
            }
            else
            {
                var result = new GetSyncResult
                {
                    WalletName = transaction.Pocket.Wallet.Name,
                    WalletMainPocketCurrencyName = transaction.Pocket.Currency.Name,
                    TransactionType = transaction.TransactionType,
                    Amount = transaction.Amount,
                    TransactionDateTime = transaction.TransactionDateTime,
                    UnitPriceInUSD = transaction.UnitPriceInUSD,
                    PairWalletName = string.IsNullOrEmpty(transaction.PairWalletName) ? string.Empty : transaction.PairWalletName,
                    PairWalletAddress = transaction.PairWalletAddress
                };

                return result;
            }
        }
    }
}