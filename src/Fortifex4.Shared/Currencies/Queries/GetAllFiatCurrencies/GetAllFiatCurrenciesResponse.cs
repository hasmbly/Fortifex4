using System.Collections.Generic;

namespace Fortifex4.Shared.Currencies.Queries.GetAllFiatCurrencies
{
    public class GetAllFiatCurrenciesResponse
    {
        public IList<FiatCurrencyDTO> FiatCurrencies { get; set; }

        public GetAllFiatCurrenciesResponse()
        {
            this.FiatCurrencies = new List<FiatCurrencyDTO>();
        }
    }
}