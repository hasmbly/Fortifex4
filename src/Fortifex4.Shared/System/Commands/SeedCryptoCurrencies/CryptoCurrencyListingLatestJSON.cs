using System;
using System.Collections.Generic;

namespace Fortifex4.Shared.System.Commands.SeedCryptoCurrencies
{
    public class CryptoCurrencyListingLatestResultJSON
    {
        public StatusJSON status { get; set; }
        public IList<CryptoCurrencyJSON> data { get; set; }
    }

    public class StatusJSON
    {
        public DateTimeOffset timestamp { get; set; }
        public int error_code { get; set; }
        public string error_message { get; set; }
        public int elapsed { get; set; }
        public int credit_count { get; set; }
        public string notice { get; set; }
    }

    public class CryptoCurrencyJSON
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
        public decimal? total_supply { get; set; }
        public PlatformJSON platform { get; set; }
        public int cmc_rank { get; set; }
        public DateTime last_updated { get; set; }
        public QuoteJSON quote { get; set; }
    }

    public class PlatformJSON
    {
        public int id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string slug { get; set; }
        public string token_address { get; set; }
    }

    public class QuoteJSON
    {
        public USDJSON USD { get; set; }
    }

    public class USDJSON
    {
        public decimal? price { get; set; }
        public decimal? volume_24h { get; set; }
        public float? percent_change_1h { get; set; }
        public float? percent_change_24h { get; set; }
        public float? percent_change_7d { get; set; }
        public decimal? market_cap { get; set; }
        public DateTimeOffset last_updated { get; set; }
    }
}