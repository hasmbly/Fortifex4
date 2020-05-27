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

namespace Fortifex4.Application.Charts.Queries.GetPortfolioByCoins
{
    public class GetPortfolioByCoinsQuery : IRequest<GetPortfolioByCoinsResult>
    {
        public string MemberUsername { get; set; }
    }

    public class GetPortfolioByCoinsQueryHandler : IRequestHandler<GetPortfolioByCoinsQuery, GetPortfolioByCoinsResult>
    {
        private readonly IFortifex4DBContext _context;
        private readonly ICryptoService _cryptoService;

        public GetPortfolioByCoinsQueryHandler(IFortifex4DBContext context, ICryptoService cryptoService)
        {
            _context = context;
            _cryptoService = cryptoService;
        }

        public async Task<GetPortfolioByCoinsResult> Handle(GetPortfolioByCoinsQuery request, CancellationToken cancellationToken)
        {
            // get member and all related stuff
            var member = await _context.Members
                .Where(x => x.MemberUsername == request.MemberUsername)
                .Include(x => x.PreferredFiatCurrency)
                .Include(x => x.PreferredCoinCurrency)
                .Include(x => x.PreferredTimeFrame)
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken);

            // check if member is null
            if (member == null)
                throw new NotFoundException(nameof(Member), request.MemberUsername);

            // init result
            var result = new GetPortfolioByCoinsResult
            {
                MemberUsername = request.MemberUsername,
                PreferredFiatCurrencySymbol = member.PreferredFiatCurrency.Symbol,
                PreferredCoinCurrencySymbol = member.PreferredCoinCurrency.Symbol
            };

            // TODO: Ini kayanya perlu diganti karena Luke minta Token dimasukkan juga ke Portfolio

            //get all member's coin pocket
            var coinPockets = await _context.Pockets
                .Where(x =>
                        x.Wallet.Owner.MemberUsername == request.MemberUsername &&
                        x.CurrencyType == CurrencyType.Coin)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // init list of CurrencyID
            List<int> currencyIDs = new List<int>();

            foreach (var coinPocket in coinPockets)
            {
                currencyIDs.Add(coinPocket.CurrencyID);
            }

            // loop with distinct of ListCurrencyID
            foreach (var currencyID in currencyIDs.Distinct())
            {
                decimal amountOfCoin = 0;

                decimal totalAmountOfCoinInFiat = 0;

                // get currency Coin
                var currency = await _context.Currencies
                    .Where(x => x.CurrencyID == currencyID)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(cancellationToken);

                // add to labels currency.Name
                result.Labels.Add(currency.Name);

                // get unit price coin in pref fiat
                var currentUnitPriceInPreferredFiatCurrency = await _cryptoService.GetUnitPriceAsync(currency.Symbol, member.PreferredFiatCurrency.Symbol);

                // get all pocket that has same CurrencyID
                var Pockets = await _context.Pockets
                    .Include(a => a.Currency)
                    .Include(b => b.Transactions)
                    .Where(x => x.CurrencyID == currencyID)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                foreach (var pocket in Pockets)
                {
                    var selectedTransactions = pocket.Transactions
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
                        amountOfCoin += transaction.Amount;
                    }
                }

                totalAmountOfCoinInFiat = amountOfCoin * currentUnitPriceInPreferredFiatCurrency;

                result.Value.Add(totalAmountOfCoinInFiat);
            }

            return result;
        }
    }
}