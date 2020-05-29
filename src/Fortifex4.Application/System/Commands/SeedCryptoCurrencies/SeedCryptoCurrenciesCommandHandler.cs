using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.File;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.System.Commands.SeedCryptoCurrencies;
using MediatR;

namespace Fortifex4.Application.System.Commands.SeedCryptoCurrencies
{
    public class SeedCryptoCurrenciesCommandHandler : IRequestHandler<SeedCryptoCurrenciesRequest, SeedCryptoCurrenciesResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IFileService _fileService;

        public SeedCryptoCurrenciesCommandHandler(IFortifex4DBContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<SeedCryptoCurrenciesResponse> Handle(SeedCryptoCurrenciesRequest request, CancellationToken cancellationToken)
        {
            var result = new SeedCryptoCurrenciesResponse();

            var cryptoCurrencyListingLatestJSON = _fileService.ReadJSONFile<CryptoCurrencyListingLatestResultJSON>(FilePath.CryptoCurrencyListingLatest);

            List<Blockchain> blockchains = new List<Blockchain>();

            #region Process Blockchain and Coin Currencies
            var coinCurrencies = cryptoCurrencyListingLatestJSON.data.Where(x => x.platform == null).ToList();

            foreach (var coinCurrencyJSON in coinCurrencies)
            {
                Blockchain blockchain = blockchains
                    .SingleOrDefault(x => x.BlockchainID == coinCurrencyJSON.id);

                if (blockchain == null)
                {
                    blockchain = new Blockchain
                    {
                        BlockchainID = coinCurrencyJSON.id,
                        Symbol = coinCurrencyJSON.symbol,
                        Name = coinCurrencyJSON.name,
                        Rank = coinCurrencyJSON.cmc_rank
                    };

                    blockchains.Add(blockchain);
                }

                blockchain.Currencies.Add(new Currency
                {
                    CoinMarketCapID = coinCurrencyJSON.id,
                    Symbol = coinCurrencyJSON.symbol,
                    Name = coinCurrencyJSON.name,
                    CurrencyType = CurrencyType.Coin,
                    IsShownInTradePair = CurrencySymbol.TradePairs.Any(x => x == coinCurrencyJSON.symbol),
                    IsForPreferredOption = CurrencySymbol.PreferredOptions.Any(x => x == coinCurrencyJSON.symbol),
                    Rank = coinCurrencyJSON.cmc_rank,
                    UnitPriceInUSD = coinCurrencyJSON.quote.USD.price ?? 0m,
                    Volume24h = coinCurrencyJSON.quote.USD.volume_24h ?? 0m,
                    PercentChange1h = coinCurrencyJSON.quote.USD.percent_change_1h ?? 0f,
                    PercentChange24h = coinCurrencyJSON.quote.USD.percent_change_24h ?? 0f,
                    PercentChange7d = coinCurrencyJSON.quote.USD.percent_change_7d ?? 0f,
                    LastUpdated = coinCurrencyJSON.quote.USD.last_updated
                });
            }
            #endregion

            #region Process Token Currencies
            var tokenCurrencies = cryptoCurrencyListingLatestJSON.data.Where(x => x.platform != null);

            foreach (var tokenCurrencyJSON in tokenCurrencies)
            {
                Blockchain blockchain = blockchains.Single(x => x.BlockchainID == tokenCurrencyJSON.platform.id);

                blockchain.Currencies.Add(new Currency
                {
                    CoinMarketCapID = tokenCurrencyJSON.id,
                    Symbol = tokenCurrencyJSON.symbol,
                    Name = tokenCurrencyJSON.name,
                    CurrencyType = CurrencyType.Token,
                    IsShownInTradePair = false,
                    IsForPreferredOption = false,
                    Rank = tokenCurrencyJSON.cmc_rank,
                    UnitPriceInUSD = tokenCurrencyJSON.quote.USD.price ?? 0m,
                    Volume24h = tokenCurrencyJSON.quote.USD.volume_24h ?? 0m,
                    PercentChange1h = tokenCurrencyJSON.quote.USD.percent_change_1h ?? 0f,
                    PercentChange24h = tokenCurrencyJSON.quote.USD.percent_change_24h ?? 0f,
                    PercentChange7d = tokenCurrencyJSON.quote.USD.percent_change_7d ?? 0f,
                    LastUpdated = tokenCurrencyJSON.quote.USD.last_updated
                });
            }
            #endregion

            if (!_context.Blockchains.Any(x => x.BlockchainID != BlockchainID.Fiat))
            {
                _context.Blockchains.AddRange(blockchains);
                await _context.SaveChangesAsync(cancellationToken);

                result.BlockchainsAdded = blockchains.Count;
                result.CoinCurrenciesAdded = blockchains.Sum(x => x.Currencies.Count(y => y.CurrencyType == CurrencyType.Coin));
                result.TokenCurrenciesAdded = blockchains.Sum(x => x.Currencies.Count(y => y.CurrencyType == CurrencyType.Token));
            }

            return result;
        }
    }
}