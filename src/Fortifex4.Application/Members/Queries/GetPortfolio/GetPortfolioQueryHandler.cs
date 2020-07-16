using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Crypto;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Members.Queries.GetPortfolio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fortifex4.Application.Members.Queries.GetPortfolio
{
    public class GetPortfolioQueryHandler : IRequestHandler<GetPortfolioRequest, GetPortfolioResponse>
    {
        private readonly ILogger<GetPortfolioQueryHandler> _logger;
        private readonly IFortifex4DBContext _context;
        private readonly ICryptoService _cryptoService;

        public GetPortfolioQueryHandler(ILogger<GetPortfolioQueryHandler> logger, IFortifex4DBContext context, ICryptoService cryptoService)
        {
            _logger = logger;
            _context = context;
            _cryptoService = cryptoService;
        }

        public async Task<GetPortfolioResponse> Handle(GetPortfolioRequest request, CancellationToken cancellationToken)
        {
            var member = await _context.Members
                .Where(x => x.MemberUsername == request.MemberUsername)
                    .Include(a => a.PreferredFiatCurrency)
                    .Include(x => x.PreferredCoinCurrency)
                    .Include(x => x.PreferredTimeFrame)
                    .Include(a => a.Owners)
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken);

            if (member == null)
                throw new NotFoundException(nameof(Member), request.MemberUsername);

            var result = new GetPortfolioResponse
            {
                MemberPreferredFiatCurrencyID = member.PreferredFiatCurrencyID,
                MemberPreferredFiatCurrencySymbol = member.PreferredFiatCurrency.Symbol,
                MemberPreferredFiatCurrencyUnitPriceInUSD = member.PreferredFiatCurrency.UnitPriceInUSD,
                MemberPreferredCoinCurrencyID= member.PreferredCoinCurrencyID,
                MemberPreferredCoinCurrencySymbol = member.PreferredCoinCurrency.Symbol,
                MemberPreferredCoinCurrencyUnitPriceInUSD = member.PreferredCoinCurrency.UnitPriceInUSD,
                MemberPreferredTimeFrameName = member.PreferredTimeFrame.Name
            };

            #region Load Crypto Currencies owned by Member
            foreach (var owner in member.Owners)
            {
                var nonFiatWallets = _context.Wallets
                    .Where(x =>
                        x.OwnerID == owner.OwnerID &&
                        x.BlockchainID != BlockchainID.Fiat)
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
                        CurrencyDTO currencyDTO = result.Currencies
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

                            currencyDTO.Transactions.Add(transactionDTO);
                        }

                        #region TotalPurchaseValueInPreferredFiatCurrency
                        if (result.MemberPreferredFiatCurrencyUnitPriceInUSD > 0)
                            currencyDTO.TotalPurchaseValueInPreferredFiatCurrency = currencyDTO.TotalPurchaseValueInUSD / result.MemberPreferredFiatCurrencyUnitPriceInUSD;
                        else
                            currencyDTO.TotalPurchaseValueInPreferredFiatCurrency = 0m;
                        #endregion

                        #region CurrentValueInPreferredCoinCurrency
                        if (currencyDTO.CurrencyID == result.MemberPreferredCoinCurrencyID)
                        {
                            currencyDTO.CurrentValueInPreferredCoinCurrency = currencyDTO.TotalAmount;
                        }
                        else
                        {
                            if (result.MemberPreferredCoinCurrencyUnitPriceInUSD > 0)
                                currencyDTO.CurrentValueInPreferredCoinCurrency = currencyDTO.TotalAmount * (currencyDTO.UnitPriceInUSD / result.MemberPreferredCoinCurrencyUnitPriceInUSD);
                            else
                                currencyDTO.CurrentValueInPreferredCoinCurrency = 0m;
                        }
                        #endregion
                    }
                }
            }
            #endregion

            var validCurrencies = result.Currencies.Where(x => x.TotalAmount > 0).ToList();

            foreach (var currencyDTO in validCurrencies)
            {
                var latestQuotesResult = await _cryptoService.GetLatestQuoteAsync(currencyDTO.Symbol, result.MemberPreferredFiatCurrencySymbol);

                if (latestQuotesResult != null)
                {
                    currencyDTO.Price = latestQuotesResult.Price;
                    currencyDTO.Volume24h = latestQuotesResult.Volume24h;
                    currencyDTO.PercentChange1h = latestQuotesResult.PercentChange1h;
                    currencyDTO.PercentChange24h = latestQuotesResult.PercentChange24h;
                    currencyDTO.PercentChange7d = latestQuotesResult.PercentChange7d;

                    #region SelectedPercentChange
                    currencyDTO.SelectedPercentChange = result.MemberPreferredTimeFrameName switch
                    {
                        TimeFrameName.OneHour => currencyDTO.PercentChange1h,
                        TimeFrameName.OneDay => currencyDTO.PercentChange24h,
                        TimeFrameName.OneWeek => currencyDTO.PercentChange7d,
                        _ => currencyDTO.PercentChangeLifetime
                    };
                    #endregion

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
                    //currencyDTO.Price = currencyDTO.UnitPriceInUSD / currencyDTO.Portfolio.MemberPreferredFiatCurrencyUnitPriceInUSD;
                    currencyDTO.Price = currencyDTO.UnitPriceInUSD / result.MemberPreferredFiatCurrencyUnitPriceInUSD;
                }

                #region Hitung Percentage Lifetime
                decimal difference = 0m;

                #region Kalau pakai Average Unit Price in Preferred Fiat Currency
                if (currencyDTO.AverageBuyPriceInPreferredFiatCurrency > 0)
                {
                    difference = currencyDTO.Price - currencyDTO.AverageBuyPriceInPreferredFiatCurrency;
                    currencyDTO.PercentChangeLifetime = Convert.ToSingle(difference / currencyDTO.AverageBuyPriceInPreferredFiatCurrency);
                }
                else
                {
                    currencyDTO.PercentChangeLifetime = 0f;
                }
                #endregion

                #region Kalau pakai First Transaction
                //var firstTransaction = currency.IncomingTransactions.FirstOrDefault();

                //if (firstTransaction != null)
                //{
                //    decimal firstTransactionUnitPriceInPreferredFiatCurrency = 1m;

                //    if (result.MemberPreferredFiatCurrencySymbol == CurrencySymbol.USD)
                //    {
                //        firstTransactionUnitPriceInPreferredFiatCurrency = firstTransaction.UnitPriceInUSD;
                //    }
                //    else
                //    {
                //        var firstTransactionUnitPriceInPreferredFiatCurrencyPriceConversionResult = _cryptoService.Send(new PriceConversionParameter
                //        {
                //            FromCurrencySymbol = CurrencySymbol.USD,
                //            ToCurrencySymbol = result.MemberPreferredFiatCurrencySymbol,
                //            Amount = firstTransaction.UnitPriceInUSD
                //        });

                //        firstTransactionUnitPriceInPreferredFiatCurrency = firstTransactionUnitPriceInPreferredFiatCurrencyPriceConversionResult.IsSuccessful ? firstTransactionUnitPriceInPreferredFiatCurrencyPriceConversionResult.Amount : 1m;
                //    }

                //    difference = currency.CurrentUnitPriceInPreferredFiatCurrency - firstTransactionUnitPriceInPreferredFiatCurrency;
                //    currencyDTO.PercentChangeLifetime = difference / firstTransactionUnitPriceInPreferredFiatCurrency;
                //}
                //else
                //{
                //    currencyDTO.PercentChangeLifetime = 0m;
                //}
                #endregion

                #endregion
            }

            return result;
        }
    }
}