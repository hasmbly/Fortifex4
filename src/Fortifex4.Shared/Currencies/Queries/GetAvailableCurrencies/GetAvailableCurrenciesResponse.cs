using System.Collections.Generic;

namespace Fortifex4.Shared.Currencies.Queries.GetAvailableCurrencies
{
    public class GetAvailableCurrenciesResponse
    {
        public IList<CurrencyDTO> Currencies { get; set; }

        public GetAvailableCurrenciesResponse()
        {
            this.Currencies = new List<CurrencyDTO>();
        }
    }
}