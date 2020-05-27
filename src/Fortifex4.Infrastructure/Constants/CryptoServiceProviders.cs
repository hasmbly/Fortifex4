namespace Fortifex4.Infrastructure.Constants
{
    public static class CryptoServiceProviders
    {
        public static class CoinMarketCap
        {
            public const string Name = "CoinMarketCap";
            public const string CryptoCurrencyMapEndpointURL = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/map?sort=cmc_rank";
            public const string ToolsPriceConversionEndpointURL = "https://pro-api.coinmarketcap.com/v1/tools/price-conversion";
            public const string CryptoCurrencyQuotesLatestEndpointURL = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest";
            public const string CryptoCurrencyListingsLatestEndpointURL = " https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest?limit=5000";
        }
    }
}