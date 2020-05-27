using System;
using Fortifex4.Domain.Enums;

namespace Fortifex4.Application.Common.Interfaces.Crypto
{
    public class CryptoCurrency
    {
        public int CurrencyID { get; set; }
        public int BlockchainID { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Slug { get; set; }
        public int Rank { get; set; }
        public decimal UnitPriceInUSD { get; set; }
        public decimal Volume24h { get; set; }
        public float PercentChange1h { get; set; }
        public float PercentChange24h { get; set; }
        public float PercentChange7d { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
        public CurrencyType CurrencyType { get; set; }
    }
}