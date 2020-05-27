using System;
using System.Collections.Generic;

namespace Fortifex4.Application.Common.Interfaces.Fiat
{
    public class FiatExchangeRateCollection
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public DateTimeOffset CollectionDateTime { get; set; }
        public IList<FiatExchangeRate> ExchangeRates { get; set; }

        public FiatExchangeRateCollection()
        {
            this.ExchangeRates = new List<FiatExchangeRate>();
        }
    }
}