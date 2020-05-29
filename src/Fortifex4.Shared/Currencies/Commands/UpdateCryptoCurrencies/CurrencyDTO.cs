using System;
using Fortifex4.Application.Enums;
using Fortifex4.Domain.Enums;

namespace Fortifex4.Shared.Currencies.Commands.UpdateCryptoCurrencies
{
    public class CurrencyDTO
    {
        public int BlockchainID { get; set; }
        public int CoinMarketCapID { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public int Rank { get; set; }
        public decimal UnitPriceInUSD { get; set; }
        public decimal Volume24h { get; set; }
        public float PercentChange1h { get; set; }
        public float PercentChange24h { get; set; }
        public float PercentChange7d { get; set; }
        public DateTimeOffset LastUpdated { get; set; }

        public UpdateStatus UpdateStatus { get; set; }
    }
}