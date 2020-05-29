using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Crypto;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Members.Queries.GetPortfolioCurrentStatus;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Members.Queries.GetPortfolioCurrentStatus
{
    public class GetPortfolioCurrentStatusQueryHandler : IRequestHandler<GetPortfolioCurrentStatusRequest, GetPortfolioCurrentStatusResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly ICryptoService _cryptoService;
        private readonly IDateTimeOffsetService _dateTimeOffset;

        public GetPortfolioCurrentStatusQueryHandler(IFortifex4DBContext context, ICryptoService cryptoService, IDateTimeOffsetService dateTimeOffset)
        {
            _context = context;
            _cryptoService = cryptoService;
            _dateTimeOffset = dateTimeOffset;
        }

        public async Task<GetPortfolioCurrentStatusResponse> Handle(GetPortfolioCurrentStatusRequest request, CancellationToken cancellationToken)
        {
            var member = await _context.Members
                .Where(x => x.MemberUsername == request.MemberUsername)
                .Include(a => a.Owners).ThenInclude(b => b.Wallets)
                .Include(a => a.PreferredFiatCurrency)
                .Include(x => x.PreferredCoinCurrency)
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken);

            if (member == null)
                throw new NotFoundException(nameof(Member), request.MemberUsername);

            var result = new GetPortfolioCurrentStatusResponse
            {
                MemberPreferredFiatCurrencySymbol = member.PreferredFiatCurrency.Symbol,
                MemberPreferredCoinCurrencySymbol = member.PreferredCoinCurrency.Symbol
            };

            #region Load Crypto Currencies owned by Member
            foreach (var owner in member.Owners)
            {
                foreach (var wallet in owner.Wallets.Where(x => x.BlockchainID != BlockchainID.Fiat))
                {
                    var mainPocket = await _context.Pockets
                        .Where(x => x.WalletID == wallet.WalletID && x.IsMain)
                        .Include(a => a.Transactions)
                        .AsNoTracking()
                        .SingleOrDefaultAsync(cancellationToken);

                    CurrencyDTO currencyDTO = result.Currencies
                        .Where(x => x.CurrencyID == mainPocket.CurrencyID)
                        .SingleOrDefault();

                    if (currencyDTO == null)
                    {
                        var currency = await _context.Currencies
                            .Where(x => x.CurrencyID == mainPocket.CurrencyID)
                            .AsNoTracking()
                            .SingleOrDefaultAsync(cancellationToken);

                        currencyDTO = new CurrencyDTO
                        {
                            CurrencyID = currency.CurrencyID,
                            Symbol = currency.Symbol,
                            Name = currency.Name
                        };

                        result.Currencies.Add(currencyDTO);
                    }

                    var selectedTransactions = mainPocket.Transactions
                            .Where(x =>
                                x.TransactionType == TransactionType.ExternalTransferIN ||
                                x.TransactionType == TransactionType.ExternalTransferOUT ||
                                x.TransactionType == TransactionType.BuyIN ||
                                x.TransactionType == TransactionType.BuyOUT ||
                                x.TransactionType == TransactionType.SellIN ||
                                x.TransactionType == TransactionType.SellOUT ||
                                x.TransactionType == TransactionType.SyncTransactionIN ||
                                x.TransactionType == TransactionType.SyncTransactionOUT)
                            .OrderBy(o => o.TransactionDateTime)
                            .ToList();

                    foreach (var transaction in selectedTransactions)
                    {
                        TransactionDTO transactionDTO = new TransactionDTO
                        {
                            TransactionID = transaction.TransactionID,
                            Amount = transaction.Amount
                        };

                        currencyDTO.Transactions.Add(transactionDTO);
                    }
                }
            }
            #endregion

            foreach (var currency in result.Currencies)
            {
                #region Convert Coin to Coin
                if (currency.Symbol == result.MemberPreferredCoinCurrencySymbol)
                {
                    currency.CurrentValueInPreferredCoinCurrency = currency.TotalAmount;
                }
                else
                {
                    currency.CurrentValueInPreferredCoinCurrency = await _cryptoService.ConvertAsync(
                        currency.Symbol,
                        result.MemberPreferredCoinCurrencySymbol,
                        currency.TotalAmount);
                }
                #endregion

                #region Get Current Unit Price in Fiat
                currency.CurrentUnitPriceInPreferredFiatCurrency = await _cryptoService.GetUnitPriceAsync(currency.Symbol, result.MemberPreferredFiatCurrencySymbol);
                #endregion
            }

            return result;
        }
    }
}