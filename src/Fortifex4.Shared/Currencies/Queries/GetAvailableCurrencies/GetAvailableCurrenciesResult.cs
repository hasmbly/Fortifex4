using System.Collections.Generic;

namespace Fortifex4.Application.Currencies.Queries.GetAvailableCurrencies
{
    public class GetAvailableCurrenciesResult
    {
        public IList<CurrencyDTO> Currencies { get; set; }

        public GetAvailableCurrenciesResult()
        {
            this.Currencies = new List<CurrencyDTO>();
        }
    }
}