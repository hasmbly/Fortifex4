using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.ExternalTransfers.Queries.GetExternalTransfer
{
    public class GetExternalTransferQuery : IRequest<GetExternalTransferResult>
    {
        public int TransactionID { get; set; }
    }

    public class GetExternalTransferQueryHandler : IRequestHandler<GetExternalTransferQuery, GetExternalTransferResult>
    {
        private readonly IFortifex4DBContext _context;

        public GetExternalTransferQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetExternalTransferResult> Handle(GetExternalTransferQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions
                .Where(x => x.TransactionID == request.TransactionID)
                .Include(a => a.Pocket)
                    .ThenInclude(b => b.Wallet)
                    .ThenInclude(c => c.Owner)
                    .ThenInclude(d => d.Provider)
                .Include(a => a.Pocket)
                    .ThenInclude(b => b.Currency)
                .SingleOrDefaultAsync(cancellationToken);

            if (transaction == null)
                throw new NotFoundException(nameof(Transaction), request.TransactionID);

            return new GetExternalTransferResult
            {
                WalletName = transaction.Pocket.Wallet.Name,
                WalletOwnerProviderName = transaction.Pocket.Wallet.Owner.Provider.Name,
                WalletMainPocketCurrencyName = transaction.Pocket.Currency.Name,
                WalletMainPocketCurrencySymbol = transaction.Pocket.Currency.Symbol,
                TransactionType = transaction.TransactionType,
                Amount = transaction.Amount,
                UnitPriceInUSD = transaction.UnitPriceInUSD,
                TransactionDateTime = transaction.TransactionDateTime,
                PairWalletName = transaction.PairWalletName,
                PairWalletAddress = transaction.PairWalletAddress
            };
        }
    }
}