using System;
using System.Reflection;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces.Fiat;
using Fortifex4.Domain.Constants;
using Fortifex4.Infrastructure.Common;
using Fortifex4.Infrastructure.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Fortifex4.Infrastructure.Fiat.Fixer
{
    public class FixerFiatService : IFiatService
    {
        private readonly ILogger<FixerFiatService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string AccessKey;

        public FixerFiatService(ILogger<FixerFiatService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            AccessKey = _configuration[ConfigurationKey.Fiat.Fixer.APIKey];
        }

        public async Task<FiatCurrencyCollection> GetFiatCurrencyCollectionAsync()
        {
            var result = new FiatCurrencyCollection();

            string uri = $"{FiatServiceProviders.Fixer.SymbolsEndpointURL}?access_key={AccessKey}";

            _logger.LogDebug($"{nameof(GetFiatCurrencyCollectionAsync)}");
            _logger.LogDebug(uri);

            var symbolsResultJSON = await ExternalWebAPIRequestor.GetAsync<SymbolsResultJSON>(uri);

            if (symbolsResultJSON.success)
            {
                foreach (PropertyInfo property in symbolsResultJSON.symbols.GetType().GetProperties())
                {
                    string fiatSymbol = property.Name;
                    string fiatName = property.GetValue(symbolsResultJSON.symbols).ToString();

                    result.Currencies.Add(new FiatCurrency
                    {
                        Symbol = fiatSymbol,
                        Name = fiatName
                    });
                }
            }
            else
            {
                result = null;
            }

            return result;
        }

        public async Task<FiatExchangeRateCollection> GetFiatExchangeRateCollectionAsync(string baseCurrencySymbol)
        {
            var result = new FiatExchangeRateCollection();

            string uri = $"{FiatServiceProviders.Fixer.LatestEndpointURL}?access_key={AccessKey}&base={baseCurrencySymbol}";

            _logger.LogDebug($"{nameof(GetFiatExchangeRateCollectionAsync)}");
            _logger.LogDebug(uri);

            var latestResultJSON = await ExternalWebAPIRequestor.GetAsync<LatestResultJSON>(uri);

            if (latestResultJSON.success)
            {
                result.IsSuccessful = true;
                result.CollectionDateTime = DateTimeOffset.FromUnixTimeSeconds(latestResultJSON.timestamp);

                foreach (PropertyInfo property in latestResultJSON.rates.GetType().GetProperties())
                {
                    string fiatSymbol = property.Name;
                    decimal fiatValue = Convert.ToDecimal(property.GetValue(latestResultJSON.rates));

                    result.ExchangeRates.Add(new FiatExchangeRate
                    {
                        Symbol = fiatSymbol,
                        Value = fiatValue
                    });
                }
            }
            else
            {
                result = null;
            }

            return result;
        }

        public async Task<decimal> ConvertAsync(string fromCurrencySymbol, string toCurrencySymbol, decimal amount)
        {
            var convertedAmount = 0m;

            string uri = $"{FiatServiceProviders.Fixer.ConvertEndpointURL}?access_key={AccessKey}&from={fromCurrencySymbol}&to={toCurrencySymbol}&amount={Math.Abs(amount)}";

            _logger.LogDebug($"{nameof(ConvertAsync)}");
            _logger.LogDebug(uri);

            var convertResultJSON = await ExternalWebAPIRequestor.GetAsync<ConvertResultJSON>(uri);

            if (convertResultJSON.success)
            {
                convertedAmount = convertResultJSON.result;
            }

            return convertedAmount;
        }

        public async Task<decimal> GetUnitPriceAsync(string fromCurrencySymbol, string toCurrencySymbol)
        {
            var convertedAmount = 0m;

            string uri = $"{FiatServiceProviders.Fixer.ConvertEndpointURL}?access_key={AccessKey}&from={fromCurrencySymbol}&to={toCurrencySymbol}&amount=1";

            _logger.LogDebug($"{nameof(GetUnitPriceAsync)}");
            _logger.LogDebug(uri);

            var convertResultJSON = await ExternalWebAPIRequestor.GetAsync<ConvertResultJSON>(uri);

            if (convertResultJSON.success)
            {
                convertedAmount = convertResultJSON.result;
            }

            return convertedAmount;
        }
    }
}