using System;

namespace Fortifex4.Application.Common.Interfaces.Crypto
{
    public class CryptoLatestQuotesResult
    {
        public int Rank { get; set; }
        public decimal Price { get; set; }
        public decimal Volume24h { get; set; }
        public float PercentChange1h { get; set; }
        public float PercentChange24h { get; set; }
        public float PercentChange7d { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
    }
}