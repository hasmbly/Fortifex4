using System;
using System.Collections.Generic;

namespace Fortifex4.Infrastructure.Crypto.CoinMarketCap
{
    public class CryptoCurrencyQuotesLatestResultJSON
    {
        public StatusJSON status { get; set; }
        public IDictionary<string, CryptoCurrencyQuotesLatestData> data { get; set; }
    }

    public class CryptoCurrencyQuotesLatestData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string slug { get; set; }
        public int num_market_pairs { get; set; }
        public DateTimeOffset date_added { get; set; }
        public IList<string> tags { get; set; }
        public decimal? max_supply { get; set; }
        public decimal? circulating_supply { get; set; }
        public decimal total_supply { get; set; }
        public int is_active { get; set; }
        public PlatformJSON platform { get; set; }
        public int? cmc_rank { get; set; }
        public int is_fiat { get; set; }
        public DateTimeOffset last_updated { get; set; }
        public IDictionary<string, CryptoCurrencyQuotesLatestQuoteJSON> quote { get; set; }
    }

    public class CryptoCurrencyQuotesLatestQuoteJSON
    {
        public decimal price { get; set; }
        public decimal? volume_24h { get; set; }
        public float? percent_change_1h { get; set; }
        public float? percent_change_24h { get; set; }
        public float? percent_change_7d { get; set; }
        public decimal? market_cap { get; set; }
        public DateTimeOffset last_updated { get; set; }
    }
}