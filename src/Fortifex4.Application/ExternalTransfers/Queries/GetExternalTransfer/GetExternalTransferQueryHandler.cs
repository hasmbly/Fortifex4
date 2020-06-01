using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.Constants;
using Fortifex4.Shared.ExternalTransfers.Queries.GetExternalTransfer;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.ExternalTransfers.Queries.GetExternalTransfer
{
    public class GetExternalTransferQueryHandler : IRequestHandler<GetExternalTransferRequest, GetExternalTransferResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetExternalTransferQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetExternalTransferResponse> Handle(GetExternalTransferRequest request, CancellationToken cancellationToken)
        {
            var result = new GetExternalTransferResponse();

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
            {
                result.IsSuccessful = false;
                result.ErrorMessage = ErrorMessage.TransactionNotFound;

                return result;
            }

            result.IsSuccessful = true;
            result.WalletName = transaction.Pocket.Wallet.Name;
            result.WalletOwnerProviderName = transaction.Pocket.Wallet.Owner.Provider.Name;
            result.WalletMainPocketCurrencyName = transaction.Pocket.Currency.Name;
            result.WalletMainPocketCurrencySymbol = transaction.Pocket.Currency.Symbol;
            result.TransactionType = transaction.TransactionType;
            result.Amount = transaction.Amount;
            result.UnitPriceInUSD = transaction.UnitPriceInUSD;
            result.TransactionDateTime = transaction.TransactionDateTime;
            result.PairWalletName = transaction.PairWalletName;
            result.PairWalletAddress = transaction.PairWalletAddress;

            result.IsSuccessful = true;

            return result;
        }
    }
}