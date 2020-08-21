using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces.Crypto;
using Fortifex4.Domain.Constants;
using Fortifex4.Domain.Enums;
using Fortifex4.Infrastructure.Common;
using Fortifex4.Infrastructure.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Fortifex4.Infrastructure.Crypto.CoinMarketCap
{
    public class CoinMarketCapCryptoService : ICryptoService
    {
        private readonly ILogger<CoinMarketCapCryptoService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string APIKey;

        public CoinMarketCapCryptoService(ILogger<CoinMarketCapCryptoService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            APIKey = _configuration[ConfigurationKey.Crypto.CoinMarketCap.APIKey];
        }

        public async Task<CryptoBlockchainCollection> GetCryptoBlockchainCollectionAsync()
        {
            var result = new CryptoBlockchainCollection();

            string uri = $"{CryptoServiceProviders.CoinMarketCap.CryptoCurrencyListingsLatestEndpointURL}";

            _logger.LogDebug($"{nameof(GetCryptoBlockchainCollectionAsync)}");
            _logger.LogDebug(uri);

            IDictionary<string, string> additionalHttpHeaders = new Dictionary<string, string>
            {
                { "X-CMC_PRO_API_KEY", APIKey }
            };

            var cryptoCurrencyListingLatestResultJSON = await ExternalWebAPIRequestor.GetAsync<CryptoCurrencyListingsLatestResultJSON>(uri, additionalHttpHeaders);

            if (cryptoCurrencyListingLatestResultJSON.status.error_code == 0)
            {
                #region Process Coin Currencies
                var coinCurrencies = cryptoCurrencyListingLatestResultJSON.data.Where(x => x.platform == null);

                foreach (var coinCurrencyJSON in coinCurrencies)
                {
                    CryptoBlockchain cryptoBlockchain = result.Blockchains
                        .SingleOrDefault(x => x.BlockchainID == coinCurrencyJSON.id);

                    if (cryptoBlockchain == null)
                    {
                        cryptoBlockchain = new CryptoBlockchain
                        {
                            BlockchainID = coinCurrencyJSON.id,
                            Name = coinCurrencyJSON.name,
                            Symbol = coinCurrencyJSON.symbol,
                            Slug = coinCurrencyJSON.slug,
                            Rank = coinCurrencyJSON.cmc_rank,
                        };

                        result.Blockchains.Add(cryptoBlockchain);
                    }

                    cryptoBlockchain.Currencies.Add(new CryptoCurrency
                    {
                        CurrencyID = coinCurrencyJSON.id,
                        BlockchainID = cryptoBlockchain.BlockchainID,
                        Name = coinCurrencyJSON.name,
                        Symbol = coinCurrencyJSON.symbol,
                        Slug = coinCurrencyJSON.slug,
                        Rank = coinCurrencyJSON.cmc_rank,
                        UnitPriceInUSD = coinCurrencyJSON.quote.USD.price ?? 0m,
                        Volume24h = coinCurrencyJSON.quote.USD.volume_24h ?? 0m,
                        PercentChange1h = coinCurrencyJSON.quote.USD.percent_change_1h ?? 0f,
                        PercentChange24h = coinCurrencyJSON.quote.USD.percent_change_24h ?? 0f,
                        PercentChange7d = coinCurrencyJSON.quote.USD.percent_change_7d ?? 0f,
                        LastUpdated = coinCurrencyJSON.quote.USD.last_updated,
                        CurrencyType = CurrencyType.Coin
                    });
                }
                #endregion

                #region Process Token Currencies
                var tokenCurrencies = cryptoCurrencyListingLatestResultJSON.data.Where(x => x.platform != null);

                foreach (var tokenCurrencyJSON in tokenCurrencies)
                {
                    CryptoBlockchain cryptoBlockchain = result.Blockchains.SingleOrDefault(x => x.BlockchainID == tokenCurrencyJSON.platform.id);

                    if (cryptoBlockchain == null)
                    {
                        _logger.LogDebug($"tokenCurrencyJSON.id: {tokenCurrencyJSON.id}");
                        _logger.LogDebug($"tokenCurrencyJSON.platform.id: {tokenCurrencyJSON.platform.id}");
                    }
                    else
                    {

                        cryptoBlockchain.Currencies.Add(new CryptoCurrency
                        {
                            CurrencyID = tokenCurrencyJSON.id,
                            BlockchainID = cryptoBlockchain.BlockchainID,
                            Name = tokenCurrencyJSON.name,
                            Symbol = tokenCurrencyJSON.symbol,
                            Slug = tokenCurrencyJSON.slug,
                            Rank = tokenCurrencyJSON.cmc_rank,
                            UnitPriceInUSD = tokenCurrencyJSON.quote.USD.price ?? 0m,
                            Volume24h = tokenCurrencyJSON.quote.USD.volume_24h ?? 0m,
                            PercentChange1h = tokenCurrencyJSON.quote.USD.percent_change_1h ?? 0f,
                            PercentChange24h = tokenCurrencyJSON.quote.USD.percent_change_24h ?? 0f,
                            PercentChange7d = tokenCurrencyJSON.quote.USD.percent_change_7d ?? 0f,
                            LastUpdated = tokenCurrencyJSON.quote.USD.last_updated,
                            CurrencyType = CurrencyType.Token
                        });
                    }
                }
                #endregion
            }
            else
            {
                result = null;
            }

            return result;
        }

        public async Task<CryptoLatestQuotesResult> GetLatestQuoteAsync(string fromCurrencySymbol, string toCurrencySymbol)
        {
            var result = new CryptoLatestQuotesResult();

            string uri = $"{CryptoServiceProviders.CoinMarketCap.CryptoCurrencyQuotesLatestEndpointURL}?symbol={fromCurrencySymbol}&convert={toCurrencySymbol}";

            _logger.LogDebug($"{nameof(GetLatestQuoteAsync)}");
            _logger.LogDebug(uri);

            IDictionary<string, string> additionalHttpHeaders = new Dictionary<string, string>
            {
                { "X-CMC_PRO_API_KEY", APIKey }
            };

            try
            {
                var cryptoCurrencyQuotesLatestResultJSON = await ExternalWebAPIRequestor.GetAsync<CryptoCurrencyQuotesLatestResultJSON>(uri, additionalHttpHeaders);

                if (cryptoCurrencyQuotesLatestResultJSON.status.error_code == 0)
                {
                    foreach (var data in cryptoCurrencyQuotesLatestResultJSON.data)
                    {
                        if (data.Key == fromCurrencySymbol)
                        {
                            foreach (var quote in data.Value.quote)
                            {
                                if (quote.Key == toCurrencySymbol)
                                {
                                    result = new CryptoLatestQuotesResult
                                    {
                                        Rank = data.Value.cmc_rank ?? 0,
                                        Price = quote.Value.price,
                                        Volume24h = quote.Value.volume_24h ?? 0m,
                                        PercentChange1h = quote.Value.percent_change_1h ?? 0f,
                                        PercentChange24h = quote.Value.percent_change_24h ?? 0f,
                                        PercentChange7d = quote.Value.percent_change_7d ?? 0f,
                                        LastUpdated = quote.Value.last_updated,
                                    };
                                }
                            }
                        }
                    }
                }
                else
                {
                    result = null;
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.BadRequest)
            {
                //result = null;
                throw;
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode != HttpStatusCode.OK)
            {
                //result = null;
                throw;
            }

            return result;
        }

        public async Task<decimal> ConvertAsync(string fromCurrencySymbol, string toCurrencySymbol, decimal amount)
        {
            if (amount == 0m)
                return 0m;

            decimal convertedAmount = 0m;

            bool isMinus = amount < 0;

            string uri = $"{CryptoServiceProviders.CoinMarketCap.ToolsPriceConversionEndpointURL}?symbol={fromCurrencySymbol}&convert={toCurrencySymbol}&amount={Math.Abs(amount)}";

            // TODO: Cek with Hasbi
            // sementara pake ini, karena ada error jika amount nya mengandung comma dan masih blum bisa solve
            //string uri = $"{CryptoServiceProviders.CoinMarketCap.ToolsPriceConversionEndpointURL}?symbol={fromCurrencySymbol}&convert={toCurrencySymbol}&amount=1";

            _logger.LogDebug($"{nameof(ConvertAsync)}");
            _logger.LogDebug(uri);

            IDictionary<string, string> additionalHttpHeaders = new Dictionary<string, string>
            {
                { "X-CMC_PRO_API_KEY", APIKey }
            };

            try
            {
                var priceConversionResultJSON = await ExternalWebAPIRequestor.GetAsync<PriceConversionResultJSON>(uri, additionalHttpHeaders);

                if (priceConversionResultJSON.status.error_code == 0)
                {
                    foreach (var quote in priceConversionResultJSON.data.quote)
                    {
                        if (quote.Key == toCurrencySymbol)
                        {
                            convertedAmount = quote.Value.price;
                            break;
                        }
                    }
                }

            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.BadRequest)
            {
                convertedAmount = 0m;
            }

            return isMinus ? -convertedAmount : convertedAmount;
        }

        public async Task<decimal> GetUnitPriceAsync(string fromCurrencySymbol, string toCurrencySymbol)
        {
            decimal unitPrice = 0m;

            string uri = $"{CryptoServiceProviders.CoinMarketCap.ToolsPriceConversionEndpointURL}?symbol={fromCurrencySymbol}&convert={toCurrencySymbol}&amount=1";

            _logger.LogDebug($"{nameof(GetUnitPriceAsync)}");
            _logger.LogDebug(uri);

            IDictionary<string, string> additionalHttpHeaders = new Dictionary<string, string>
            {
                { "X-CMC_PRO_API_KEY", APIKey }
            };

            try
            {
                var priceConversionResultJSON = await ExternalWebAPIRequestor.GetAsync<PriceConversionResultJSON>(uri, additionalHttpHeaders);

                if (priceConversionResultJSON.status.error_code == 0)
                {
                    foreach (var quote in priceConversionResultJSON.data.quote)
                    {
                        if (quote.Key == toCurrencySymbol)
                        {
                            unitPrice = quote.Value.price;
                            break;
                        }
                    }
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.BadRequest)
            {
                unitPrice = 0m;
            }

            return unitPrice;
        }
    }
}