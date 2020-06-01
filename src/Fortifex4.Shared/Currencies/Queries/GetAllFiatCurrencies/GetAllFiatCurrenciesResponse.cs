using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Currencies.Queries.GetAllFiatCurrencies
{
    public class GetAllFiatCurrenciesResponse : GeneralResponse
    {
        public IList<FiatCurrencyDTO> FiatCurrencies { get; set; }

        public GetAllFiatCurrenciesResponse()
        {
            this.FiatCurrencies = new List<FiatCurrencyDTO>();
        }
    }
}