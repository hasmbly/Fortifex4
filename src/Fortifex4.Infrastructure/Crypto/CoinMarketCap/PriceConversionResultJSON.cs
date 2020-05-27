using System;
using System.Collections.Generic;

namespace Fortifex4.Infrastructure.Crypto.CoinMarketCap
{
    public class PriceConversionResultJSON
    {
        public StatusJSON status { get; set; }
        public PriceConversionDataJSON data { get; set; }
    }    

    public class PriceConversionDataJSON
    {
        public int id { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public decimal amount { get; set; }
        public DateTimeOffset last_updated { get; set; }
        public IDictionary<string, PriceConversionQuoteJSON> quote { get; set; }
    }

    public class PriceConversionQuoteJSON
    {
        public decimal price { get; set; }
        public DateTimeOffset last_updated { get; set; }
    }
}