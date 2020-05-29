using System.Collections.Generic;

namespace Fortifex4.Shared.Currencies.Commands.UpdateFiatCurrencyCoinMarketCapIDs
{
    public class UpdateFiatCurrencyCoinMarketCapIDsResponse
    {
        public IList<CurrencyDTO> Currencies { get; set; }

        public UpdateFiatCurrencyCoinMarketCapIDsResponse()
        {
            this.Currencies = new List<CurrencyDTO>();
        }
    }
}