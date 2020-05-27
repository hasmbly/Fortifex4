using System;
using System.Collections.Generic;

namespace Fortifex4.Infrastructure.Crypto.CoinMarketCap
{
    public class CryptoCurrencyMapResultJSON
    {
        public StatusJSON status { get; set; }
        public IList<CryptoCurrencyMapDataJSON> data { get; set; }
    }

    public class CryptoCurrencyMapDataJSON
    {
        public int id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string slug { get; set; }
        public int is_active { get; set; }
        public int rank { get; set; }
        public DateTime first_historical_data { get; set; }
        public DateTime last_historical_data { get; set; }
        public PlatformJSON platform { get; set; }
    }
}