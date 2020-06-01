using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Currencies.Queries.GetAvailableCurrencies
{
    public class GetAvailableCurrenciesResponse : GeneralResponse
    {
        public IList<CurrencyDTO> Currencies { get; set; }

        public GetAvailableCurrenciesResponse()
        {
            this.Currencies = new List<CurrencyDTO>();
        }
    }
}