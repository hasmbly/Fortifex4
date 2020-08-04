using Fortifex4.Domain.Common;
using Fortifex4.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fortifex4.Domain.Entities
{
    public class Currency : AuditableEntity
    {
        public int CurrencyID { get; set; }
        public int BlockchainID { get; set; }
        public int CoinMarketCapID { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public bool IsShownInTradePair { get; set; }
        public bool IsForPreferredOption { get; set; }
        public int Rank { get; set; }
        public decimal UnitPriceInUSD { get; set; }
        public decimal Volume24h { get; set; }
        public float PercentChange1h { get; set; }
        public float PercentChange24h { get; set; }
        public float PercentChange7d { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
        public bool IsFromCoinMarketCap { get; set; }

        public Blockchain Blockchain { get; set; }

        public IList<Pocket> Pockets { get; private set; }

        public Currency()
        {
            this.Pockets = new List<Pocket>();
        }
    }

    public static class CurrencySymbol
    {
        public const string USD = "USD";
        public const string BTC = "BTC";
        public const string ETH = "ETH";
        public const string DOGE = "DOGE";
        public const string STEEM = "STEEM";
        public const string HIVE = "HIVE";

        public static readonly IList<string> TradePairs = new List<string> { BTC, ETH, USD };
        public static readonly IList<string> PreferredOptions = new List<string> { BTC, ETH };
        public static readonly IList<string> SynchronizationOptions = new List<string> { BTC, ETH, DOGE, STEEM, HIVE };

        public static bool IsSynchronizable(string currencySymbol) => SynchronizationOptions.Any(x => x == currencySymbol);
    }

    public static class ProblematicCurrency
    {
        public static readonly IList<int> ByIDs = new List<int> { 5819 };
    }
}