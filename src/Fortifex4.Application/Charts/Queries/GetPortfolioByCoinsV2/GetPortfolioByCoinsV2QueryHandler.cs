using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Crypto;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Charts.Queries.GetPortfolioByCoinsV2;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fortifex4.Application.Charts.Queries.GetPortfolioByCoinsV2
{
    public class GetPortfolioByCoinsV2QueryHandler : IRequestHandler<GetPortfolioByCoinsV2Request, GetPortfolioByCoinsV2Response>
    {
        private readonly ILogger<GetPortfolioByCoinsV2QueryHandler> _logger;
        private readonly IFortifex4DBContext _context;
        private readonly ICryptoService _cryptoService;

        public GetPortfolioByCoinsV2QueryHandler(ILogger<GetPortfolioByCoinsV2QueryHandler> logger, IFortifex4DBContext context, ICryptoService cryptoService)
        {
            _logger = logger;
            _context = context;
            _cryptoService = cryptoService;
        }

        public async Task<GetPortfolioByCoinsV2Response> Handle(GetPortfolioByCoinsV2Request request, CancellationToken cancellationToken)
        {
            var member = await _context.Members
                .Where(x => x.MemberUsername == request.MemberUsername)
                    .Include(a => a.PreferredFiatCurrency)
                    .Include(b => b.Owners)
                        .ThenInclude(c => c.Provider)
                    .Include(c => c.Owners)
                        .ThenInclude(d => d.Wallets)
                        .ThenInclude(e => e.Pockets)
                        .ThenInclude(f => f.Transactions)
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken);

            if (member == null)
                throw new NotFoundException(nameof(Member), request.MemberUsername);

            var result = new GetPortfolioByCoinsV2Response
            {
                MemberPreferredFiatCurrencySymbol = member.PreferredFiatCurrency.Symbol,
                MemberPreferredFiatCurrencyUnitPriceInUSD = member.PreferredFiatCurrency.UnitPriceInUSD
            };

            foreach (var owner in member.Owners)
            {
                var nonFiatWallets = _context.Wallets
                    .Where(x =>
                        x.OwnerID == owner.OwnerID &&
                        x.BlockchainID != BlockchainID.Fiat)
                    .Include(a => a.Blockchain)
                    .AsNoTracking()
                    .ToList();

                foreach (var wallet in nonFiatWallets)
                {
                    var pockets = await _context.Pockets
                        .Where(x =>
                            x.WalletID == wallet.WalletID &&
                            x.Currency.UnitPriceInUSD > 0)
                        .Include(a => a.Transactions)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    foreach (var pocket in pockets)
                    {
                        var currencyDTO = result.Currencies
                            .Where(x => x.CurrencyID == pocket.CurrencyID)
                            .SingleOrDefault();

                        if (currencyDTO == null)
                        {
                            var currency = await _context.Currencies
                                .Where(x => x.CurrencyID == pocket.CurrencyID)
                                .AsNoTracking()
                                .SingleOrDefaultAsync(cancellationToken);

                            currencyDTO = new CurrencyDTO
                            {
                                CurrencyID = currency.CurrencyID,
                                Symbol = currency.Symbol,
                                Name = currency.Name,
                                UnitPriceInUSD = currency.UnitPriceInUSD,
                                CurrencyType = currency.CurrencyType
                            };

                            result.Currencies.Add(currencyDTO);
                        }

                        var selectedTransactions = pocket.Transactions
                                .Where(x =>
                                    x.TransactionType == TransactionType.StartingBalance ||
                                    x.TransactionType == TransactionType.BuyIN ||
                                    x.TransactionType == TransactionType.BuyOUT ||
                                    x.TransactionType == TransactionType.SellIN ||
                                    x.TransactionType == TransactionType.SellOUT ||
                                    x.TransactionType == TransactionType.ExternalTransferIN ||
                                    x.TransactionType == TransactionType.ExternalTransferOUT ||
                                    x.TransactionType == TransactionType.SyncBalanceImport ||
                                    x.TransactionType == TransactionType.SyncTransactionIN ||
                                    x.TransactionType == TransactionType.SyncTransactionOUT)
                                .OrderBy(o => o.TransactionDateTime)
                                .ToList();

                        foreach (var transaction in selectedTransactions)
                        {
                            TransactionDTO transactionDTO = new TransactionDTO
                            {
                                TransactionID = transaction.TransactionID,
                                Amount = transaction.Amount,
                                TransactionType = transaction.TransactionType,
                                TransactionDateTime = transaction.TransactionDateTime,
                                UnitPriceInUSD = transaction.UnitPriceInUSD
                            };

                            currencyDTO.TransactionDTO.Add(transactionDTO);
                        }
                    }
                }
            }


            var validCurrencies = result.Currencies.Where(x => x.TotalAmount > 0).ToList();

            foreach (var currencyDTO in validCurrencies)
            {
                var latestQuotesResult = await _cryptoService.GetLatestQuoteAsync(currencyDTO.Symbol, result.MemberPreferredFiatCurrencySymbol);

                if (latestQuotesResult != null)
                {
                    currencyDTO.Price = latestQuotesResult.Price;

                    // Sekalian update Currency kalau MemberPreferredFiatCurrencySymbol adalah USD
                    if (result.MemberPreferredFiatCurrencySymbol == CurrencySymbol.USD)
                    {
                        var currency = await _context.Currencies
                            .Where(x => x.CurrencyID == currencyDTO.CurrencyID)
                            .SingleAsync(cancellationToken);

                        currency.Rank = latestQuotesResult.Rank;
                        currency.UnitPriceInUSD = latestQuotesResult.Price;
                        currency.Volume24h = latestQuotesResult.Volume24h;
                        currency.PercentChange1h = latestQuotesResult.PercentChange1h;
                        currency.PercentChange24h = latestQuotesResult.PercentChange24h;
                        currency.PercentChange7d = latestQuotesResult.PercentChange7d;
                        currency.LastUpdated = latestQuotesResult.LastUpdated;

                        await _context.SaveChangesAsync(cancellationToken);

                        currencyDTO.UnitPriceInUSD = latestQuotesResult.Price;
                    }
                }
                else
                {
                    currencyDTO.Price = currencyDTO.UnitPriceInUSD / result.MemberPreferredFiatCurrencyUnitPriceInUSD;
                }
            }

            result.IsSuccessful = true;

            return result;
        }
    }
}