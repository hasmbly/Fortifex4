using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.StartingBalance.Queries.GetStartingBalance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.StartingBalance.Queries.GetStartingBalance
{
    public class GetStartingBalanceQueryHandler : IRequestHandler<GetStartingBalanceRequest, GetStartingBalanceResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetStartingBalanceQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetStartingBalanceResponse> Handle(GetStartingBalanceRequest request, CancellationToken cancellationToken)
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

            var result = new GetStartingBalanceResponse
            {
                WalletName = transaction.Pocket.Wallet.Name,
                WalletOwnerProviderName = transaction.Pocket.Wallet.Owner.Provider.Name,
                WalletMainPocketCurrencyName = transaction.Pocket.Currency.Name,
                WalletMainPocketCurrencySymbol = transaction.Pocket.Currency.Symbol,
                TransactionType = transaction.TransactionType,
                Amount = transaction.Amount,
                TransactionDateTime = transaction.TransactionDateTime,
                UnitPriceInUSD = transaction.UnitPriceInUSD,
                PairWalletName = transaction.PairWalletName,
                PairWalletAddress = transaction.PairWalletAddress
            };

            result.IsSuccessful = true;

            return result;
        }
    }    
}