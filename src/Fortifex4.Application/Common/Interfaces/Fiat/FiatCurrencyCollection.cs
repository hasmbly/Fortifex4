using System.Collections.Generic;

namespace Fortifex4.Application.Common.Interfaces.Fiat
{
    public class FiatCurrencyCollection
    {
        public IList<FiatCurrency> Currencies { get; set; }

        public FiatCurrencyCollection()
        {
            this.Currencies = new List<FiatCurrency>();
        }
    }
}