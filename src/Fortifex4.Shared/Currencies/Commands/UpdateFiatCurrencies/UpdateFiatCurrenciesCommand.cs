using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Fiat;
using Fortifex4.Application.Enums;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Currencies.Commands.UpdateFiatCurrencies
{
    public class UpdateFiatCurrenciesCommand : IRequest<UpdateFiatCurrenciesResult>
    {
    }

    public class UpdateCryptoCurrenciesCommandHandler : IRequestHandler<UpdateFiatCurrenciesCommand, UpdateFiatCurrenciesResult>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IFiatService _fiatService;

        public UpdateCryptoCurrenciesCommandHandler(IFortifex4DBContext context, IFiatService fiatService)
        {
            _context = context;
            _fiatService = fiatService;
        }

        public async Task<UpdateFiatCurrenciesResult> Handle(UpdateFiatCurrenciesCommand request, CancellationToken cancellationToken)
        {
            var result = new UpdateFiatCurrenciesResult();

            Blockchain blockchain = await _context.Blockchains
                .Where(x => x.BlockchainID == BlockchainID.Fiat)
                .SingleOrDefaultAsync(cancellationToken);

            if (blockchain == null)
            {
                blockchain = new Blockchain
                {
                    BlockchainID = BlockchainID.Fiat,
                    Symbol = BlockchainSymbol.Fiat,
                    Name = BlockchainName.Fiat,
                    Rank = BlockchainRank.Fiat
                };

                _context.Blockchains.Add(blockchain);
            }

            var fiatCurrencyCollection = await _fiatService.GetFiatCurrencyCollectionAsync();
            var fiatExchangeRateCollection = await _fiatService.GetFiatExchangeRateCollectionAsync(CurrencySymbol.USD);

            if (fiatCurrencyCollection != null && fiatExchangeRateCollection != null)
            {
                foreach (var fiatCurrency in fiatCurrencyCollection.Currencies)
                {
                    Currency currency = await _context.Currencies
                        .Where(x => x.Symbol == fiatCurrency.Symbol && x.CurrencyType == CurrencyType.Fiat)
                        .SingleOrDefaultAsync(cancellationToken);

                    FiatExchangeRate fiatExchangeRate = fiatExchangeRateCollection.ExchangeRates
                        .Where(x => x.Symbol == fiatCurrency.Symbol)
                        .SingleOrDefault();

                    decimal exchangeRateInUSD = fiatExchangeRate != null ? fiatExchangeRate.Value : 0m;
                    decimal unitPriceInUSD = exchangeRateInUSD > 0 ? 1 / exchangeRateInUSD : 0m;

                    CurrencyDTO currencyDTO = new CurrencyDTO
                    {
                        Symbol = fiatCurrency.Symbol,
                        Name = fiatCurrency.Name,
                        UnitPriceInUSD = unitPriceInUSD,
                        LastUpdated = fiatExchangeRateCollection.CollectionDateTime
                    };

                    result.Currencies.Add(currencyDTO);

                    if (currency == null)
                    {
                        currency = new Currency
                        {
                            CoinMarketCapID = fiatCurrency.CurrencyID,
                            Symbol = fiatCurrency.Symbol,
                            Name = fiatCurrency.Name,
                            CurrencyType = CurrencyType.Fiat,
                            IsShownInTradePair = CurrencySymbol.TradePairs.Any(x => x == fiatCurrency.Symbol),
                            IsForPreferredOption = true,
                            UnitPriceInUSD = unitPriceInUSD,
                            LastUpdated = fiatExchangeRateCollection.CollectionDateTime
                        };

                        blockchain.Currencies.Add(currency);

                        currencyDTO.UpdateStatus = UpdateStatus.New;
                    }
                    else
                    {
                        currency.Symbol = fiatCurrency.Symbol;
                        currency.Name = fiatCurrency.Name;
                        currency.UnitPriceInUSD = unitPriceInUSD;
                        currency.LastUpdated = fiatExchangeRateCollection.CollectionDateTime;

                        currencyDTO.UpdateStatus = UpdateStatus.Updated;
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
            }            

            return result;
        }
    }
}