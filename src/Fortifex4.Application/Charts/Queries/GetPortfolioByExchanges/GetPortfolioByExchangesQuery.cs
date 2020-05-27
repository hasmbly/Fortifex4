using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Crypto;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Charts.Queries.GetPortfolioByExchanges
{
    public class GetPortfolioByExchangesQuery : IRequest<GetPortfolioByExchangesResult>
    {
        public string MemberUsername { get; set; }
    }

    public class GetPortfolioByExchangesQueryHandler : IRequestHandler<GetPortfolioByExchangesQuery, GetPortfolioByExchangesResult>
    {
        private readonly IFortifex4DBContext _context;
        private readonly ICryptoService _cryptoService;

        public GetPortfolioByExchangesQueryHandler(IFortifex4DBContext context, ICryptoService cryptoService)
        {
            _context = context;
            _cryptoService = cryptoService;
        }

        public async Task<GetPortfolioByExchangesResult> Handle(GetPortfolioByExchangesQuery request, CancellationToken cancellationToken)
        {
            var member = await _context.Members
                .Where(x => x.MemberUsername == request.MemberUsername)
                .Include(a => a.Owners).ThenInclude(b => b.Wallets)
                .Include(x => x.Owners).ThenInclude(y => y.Provider)
                .Include(x => x.PreferredFiatCurrency)
                .Include(x => x.PreferredCoinCurrency)
                .Include(x => x.PreferredTimeFrame)
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken);

            if (member == null)
                throw new NotFoundException(nameof(Member), request.MemberUsername);

            var result = new GetPortfolioByExchangesResult
            {
                MemberUsername = request.MemberUsername,
                FiatCode = member.PreferredFiatCurrency.Symbol,
                CryptoCode = member.PreferredCoinCurrency.Symbol
            };

            foreach (var owner in member.Owners)
            {
                List<decimal> amountExchangeCoin = new List<decimal>();

                result.Labels.Add(owner.Provider.Name);

                foreach (var wallet in owner.Wallets.Where(x => x.BlockchainID != BlockchainID.Fiat))
                {
                    decimal amount = 0;
                    decimal totalUnitPrice = 0;

                    var mainPocket = await _context.Pockets
                                  .Where(x => x.WalletID == wallet.WalletID && x.IsMain)
                                  .Include(a => a.Transactions)
                                  .Include(b => b.Currency)
                                  .AsNoTracking()
                                  .SingleOrDefaultAsync(cancellationToken);

                    var currentUnitPriceInPreferredFiatCurrency = await _cryptoService.GetUnitPriceAsync(mainPocket.Currency.Symbol, member.PreferredFiatCurrency.Symbol);

                    var selectedTransactions = mainPocket.Transactions
                            .Where(x =>
                                x.TransactionType == TransactionType.ExternalTransferIN ||
                                x.TransactionType == TransactionType.ExternalTransferOUT ||
                                x.TransactionType == TransactionType.BuyIN ||
                                x.TransactionType == TransactionType.BuyOUT ||
                                x.TransactionType == TransactionType.SellIN ||
                                x.TransactionType == TransactionType.SellOUT ||
                                x.TransactionType == TransactionType.SyncTransactionIN ||
                                x.TransactionType == TransactionType.SyncTransactionOUT ||
                                x.TransactionType == TransactionType.BuyOUTNonWithholding ||
                                x.TransactionType == TransactionType.SellINNonWithholding)
                            .OrderBy(o => o.TransactionDateTime)
                            .ToList();

                    foreach (var transaction in selectedTransactions)
                    {
                        amount += transaction.Amount;
                    }

                    totalUnitPrice = amount * currentUnitPriceInPreferredFiatCurrency;

                    amountExchangeCoin.Add(totalUnitPrice);
                }

                result.Value.Add(amountExchangeCoin.Sum());
            }

            return result;
        }
    }
}