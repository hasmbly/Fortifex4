using System.Collections.Generic;

namespace Fortifex4.Application.Currencies.Queries.GetAllFiatCurrencies
{
    public class GetAllFiatCurrenciesResult
    {
        public IList<FiatCurrencyDTO> FiatCurrencies { get; set; }

        public GetAllFiatCurrenciesResult()
        {
            this.FiatCurrencies = new List<FiatCurrencyDTO>();
        }
    }
}