using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Crypto;
using Fortifex4.Shared.Enums;
using Fortifex4.Domain.Entities;
using Fortifex4.Shared.Currencies.Commands.UpdateCryptoCurrencies;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fortifex4.Application.Currencies.Commands.UpdateCryptoCurrencies
{
    public class UpdateCryptoCurrenciesCommandHandler : IRequestHandler<UpdateCryptoCurrenciesRequest, UpdateCryptoCurrenciesResponse>
    {
        private readonly ILogger<UpdateCryptoCurrenciesCommandHandler> _logger;
        private readonly IFortifex4DBContext _context;
        private readonly ICryptoService _cryptoService;

        public UpdateCryptoCurrenciesCommandHandler(ILogger<UpdateCryptoCurrenciesCommandHandler> logger, IFortifex4DBContext context, ICryptoService cryptoService)
        {
            _logger = logger;
            _context = context;
            _cryptoService = cryptoService;
        }

        public async Task<UpdateCryptoCurrenciesResponse> Handle(UpdateCryptoCurrenciesRequest request, CancellationToken cancellationToken)
        {
            var result = new UpdateCryptoCurrenciesResponse();

            var cryptoBlockchainCollection = await _cryptoService.GetCryptoBlockchainCollectionAsync();

            if (cryptoBlockchainCollection != null)
            {
                result.IsSuccessful = true;

                // Segala sesuatunya harus berdasarkan CoinMarketCapID
                // Di entity Blockchain, isi BlockchainID adalah CoinMarketCapID
                foreach (var cryptoBlockchain in cryptoBlockchainCollection.Blockchains)
                {
                    Blockchain blockchain = _context.Blockchains.SingleOrDefault(x => x.BlockchainID == cryptoBlockchain.BlockchainID);

                    BlockchainDTO blockchainDTO = new BlockchainDTO
                    {
                        BlockchainID = cryptoBlockchain.BlockchainID,
                        Symbol = cryptoBlockchain.Symbol,
                        Name = cryptoBlockchain.Name,
                        Rank = cryptoBlockchain.Rank,
                    };

                    result.Blockchains.Add(blockchainDTO);

                    if (blockchain == null)
                    {
                        blockchain = new Blockchain
                        {
                            BlockchainID = cryptoBlockchain.BlockchainID,
                            Symbol = cryptoBlockchain.Symbol,
                            Name = cryptoBlockchain.Name,
                            Rank = cryptoBlockchain.Rank
                        };

                        _context.Blockchains.Add(blockchain);
                        await _context.SaveChangesAsync(cancellationToken);

                        blockchainDTO.UpdateStatus = UpdateStatus.New;
                    }
                    else
                    {
                        blockchain.Symbol = cryptoBlockchain.Symbol;
                        blockchain.Name = cryptoBlockchain.Name;
                        blockchain.Rank = cryptoBlockchain.Rank;

                        blockchainDTO.UpdateStatus = UpdateStatus.Updated;
                    }

                    foreach (var cryptoCurrency in cryptoBlockchain.Currencies)
                    {
                        CurrencyDTO currencyDTO = new CurrencyDTO
                        {
                            BlockchainID = blockchain.BlockchainID,
                            CoinMarketCapID = cryptoCurrency.CurrencyID,
                            Symbol = cryptoCurrency.Symbol,
                            Name = cryptoCurrency.Name,
                            CurrencyType = cryptoCurrency.CurrencyType,
                            Rank = cryptoCurrency.Rank,
                            UnitPriceInUSD = cryptoCurrency.UnitPriceInUSD,
                            Volume24h = cryptoCurrency.Volume24h,
                            PercentChange1h = cryptoCurrency.PercentChange1h,
                            PercentChange24h = cryptoCurrency.PercentChange24h,
                            PercentChange7d = cryptoCurrency.PercentChange7d,
                            LastUpdated = cryptoCurrency.LastUpdated
                        };

                        blockchainDTO.Currencies.Add(currencyDTO);

                        var query = _context.Currencies.Where(x => x.CoinMarketCapID == cryptoCurrency.CurrencyID);

                        var currencies = query.ToList();

                        if (currencies.Count == 0)
                        {
                            // Buat jaga-jaga kalo ada Currency yang bermasalah
                            //if (ProblematicCurrency.ByIDs.Any(x => x == currency.CurrencyID))
                            //{
                            //    continue;
                            //}

                            _logger.LogDebug($"cryptoCurrency.CurrencyID {cryptoCurrency.CurrencyID}");
                            _logger.LogDebug($"cryptoCurrency.Name {cryptoCurrency.Name}");
                            _logger.LogDebug($"cryptoCurrency.Symbol {cryptoCurrency.Symbol}");

                            var currency = new Currency
                            {
                                CoinMarketCapID = cryptoCurrency.CurrencyID,
                                Symbol = cryptoCurrency.Symbol,
                                Name = cryptoCurrency.Name,
                                CurrencyType = cryptoCurrency.CurrencyType,
                                IsShownInTradePair = CurrencySymbol.TradePairs.Any(x => x == cryptoCurrency.Symbol),
                                IsForPreferredOption = CurrencySymbol.PreferredOptions.Any(x => x == cryptoCurrency.Symbol),
                                Rank = cryptoCurrency.Rank,
                                UnitPriceInUSD = cryptoCurrency.UnitPriceInUSD,
                                Volume24h = cryptoCurrency.Volume24h,
                                PercentChange1h = cryptoCurrency.PercentChange1h,
                                PercentChange24h = cryptoCurrency.PercentChange24h,
                                PercentChange7d = cryptoCurrency.PercentChange7d,
                                LastUpdated = cryptoCurrency.LastUpdated,
                                IsFromCoinMarketCap = true
                            };

                            blockchain.Currencies.Add(currency);
                            await _context.SaveChangesAsync(cancellationToken);

                            currencyDTO.UpdateStatus = UpdateStatus.New;
                        }
                        else
                        {
                            foreach (var currency in currencies)
                            {
                                if (currency.IsFromCoinMarketCap)
                                {
                                    currency.Symbol = cryptoCurrency.Symbol;
                                    currency.Name = cryptoCurrency.Name;
                                }

                                currency.Rank = cryptoCurrency.Rank;
                                currency.UnitPriceInUSD = cryptoCurrency.UnitPriceInUSD;
                                currency.Volume24h = cryptoCurrency.Volume24h;
                                currency.PercentChange1h = cryptoCurrency.PercentChange1h;
                                currency.PercentChange24h = cryptoCurrency.PercentChange24h;
                                currency.PercentChange7d = cryptoCurrency.PercentChange7d;
                                currencyDTO.UpdateStatus = UpdateStatus.Updated;
                            }

                            await _context.SaveChangesAsync(cancellationToken);
                        }                        
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}