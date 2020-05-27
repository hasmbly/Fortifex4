using System;

namespace Fortifex4.Infrastructure.Crypto.CoinMarketCap
{
    public class StatusJSON
    {
        public DateTimeOffset timestamp { get; set; }
        public int error_code { get; set; }
        public string error_message { get; set; }
        public int elapsed { get; set; }
        public int credit_count { get; set; }
        public string notice { get; set; }
    }
}