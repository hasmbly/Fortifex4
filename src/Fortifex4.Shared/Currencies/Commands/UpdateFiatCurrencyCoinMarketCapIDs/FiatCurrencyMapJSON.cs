using System;
using System.Collections.Generic;

namespace Fortifex4.Shared.Currencies.Commands.UpdateFiatCurrencyCoinMarketCapIDs
{
    public class FiatCurrencyMapJSON
    {
        public StatusJSON status { get; set; }
        public IList<FiatCurrencyJSON> data { get; set; }
    }

    public class StatusJSON
    {
        public DateTime timestamp { get; set; }
        public int error_code { get; set; }
        public string error_message { get; set; }
        public int elapsed { get; set; }
        public int credit_count { get; set; }
        public string notice { get; set; }
    }

    public class FiatCurrencyJSON
    {
        public int id { get; set; }
        public string name { get; set; }
        public string sign { get; set; }
        public string symbol { get; set; }
        public string code { get; set; }
    }
}