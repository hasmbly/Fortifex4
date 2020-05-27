using System.Collections.Generic;

namespace Fortifex4.Application.Currencies.Commands.UpdateFiatCurrencyCoinMarketCapIDs
{
    public class UpdateFiatCurrencyCoinMarketCapIDsResult
    {
        public IList<CurrencyDTO> Currencies { get; set; }

        public UpdateFiatCurrencyCoinMarketCapIDsResult()
        {
            this.Currencies = new List<CurrencyDTO>();
        }
    }
}